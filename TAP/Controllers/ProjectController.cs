using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TAP.Data;
using TAP.Dtos;
using TAP.Models;

namespace TAP.Controllers
{
    [ApiController]
    [ApiVersion("3.0")]
    [Route("api/v{version:apiVersion}/[controller].{format}")]

    public class ProjectController : ControllerBase
    {

        private readonly IProject _repository;
        private readonly ILogger<ProjectController> _logger;
        private readonly IMapper _mapper;

        public ProjectController(ILogger<ProjectController> logger, IProject repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }


        //GET V3 allow anonymus
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<ProjectReadDTO>> GetAllProjects()
        {

            var projectItems = _repository.GetAllProjects();

            if (projectItems != null)
            {
                _logger.LogInformation("Getting " + projectItems.Count() + " projects from database.");

                return Ok(_mapper.Map<IEnumerable<ProjectReadDTO>>(projectItems));
            }
            else
            {
                _logger.LogError("Can't find any project in database.");

                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }
        }

        //GET V2 handle authorization error
        [AllowAnonymous]
        [ApiVersion("2.0")]
        [HttpGet]
        public ActionResult<IEnumerable<ProjectReadDTO>> GetAllProjectsV2()
        {
            var projectItems = _repository.GetAllProjects();

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(_repository.ErrorDescription(HttpStatusCode.Unauthorized));
            }

            if (projectItems != null)
            {
                _logger.LogInformation("Getting " + projectItems.Count() + " projects from database.");

                return Ok(_mapper.Map<IEnumerable<ProjectReadDTO>>(projectItems));
            }
            else
            {
                _logger.LogError("Can't find any project in database.");

                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }

        }

        //GET_BY_ID
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<ProjectReadDTO> GetProjectById(int id)
        {
            var projectItem = _repository.GetProjectDataById(id);

            if (projectItem != null)
            {
                _logger.LogInformation("Retreiving project " + projectItem.Name + ".");

                return Ok(_mapper.Map<ProjectReadDTO>(projectItem));
            }
            else
            {
                _logger.LogError("Project with ID " + id + " doesn't exist");


                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }
        }

        //POST 
        [Authorize]
        [HttpPost]
        public ActionResult<ProjectReadDTO> PostProject(ProjectCreateDTO project)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogError("Bad Request 400."); 

                return BadRequest(ModelState);
            }

            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Unauthorized(_repository.ErrorDescription(HttpStatusCode.Unauthorized));

            //}

            if (project != null)
            {
                var projectModel = _mapper.Map<Project>(project);
                _repository.CreateProject(projectModel);

                if (_repository.SaveChanges())
                {
                    var projectDto = _mapper.Map<ProjectReadDTO>(projectModel);

                    _logger.LogInformation("Creating project" + projectDto.Name + ".");

                    return CreatedAtRoute(new {Id = projectDto.Id}, projectDto);
                }
                else
                {
                    return StatusCode(500, new {message = "Internal Server error 500. Something went wrong"});

                }

            }
            else
            {
                _logger.LogError("Project " + project.Name + "can't be created.");

                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }

        }

        //PUT 
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult UpdateProject(int id, ProjectUpdateDTO projectUpdateDto)
        {
            var projectModelfromRepo = _repository.GetProjectDataById(id);
            if (projectModelfromRepo == null)
            {
                _logger.LogError("The project with ID: " + id + " doesn't exist.");
                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }
            else
            {
                _mapper.Map(projectUpdateDto, projectModelfromRepo);

                _logger.LogInformation("Updating project " + projectModelfromRepo.Name + ".");

                _repository.UpdateProject(projectModelfromRepo,id);

                if (!_repository.SaveChanges())
                {
                    return StatusCode(500, new { message = "Internal Server error 500." +"Something went wrong" });
                }
                else
                {
                    return NoContent();

                }

            }

        }

        //PATCH 
        [Authorize]
        [HttpPatch("{id}")]
        public ActionResult PartialProjectUpdate(int id, JsonPatchDocument<ProjectUpdateDTO> patchDoc)
        {
            var projectModelfromRepo = _repository.GetProjectDataById(id);

            if (projectModelfromRepo == null)
            {
                _logger.LogError("The project with ID " + id + " doesn't exist.");

                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }
            else
            {
                var projectToPatch = _mapper.Map<ProjectUpdateDTO>(projectModelfromRepo);

                patchDoc.ApplyTo(projectToPatch, ModelState);

                if (!TryValidateModel(projectToPatch))
                {
                    return ValidationProblem(ModelState);
                }

                _mapper.Map(projectToPatch, projectModelfromRepo);

                _repository.UpdateProject(projectModelfromRepo,id);

                _logger.LogInformation("Updated project's property.\n New Value: "
                                       + projectModelfromRepo.Id + " "
                                       + projectModelfromRepo.Name + " "
                                       + projectModelfromRepo.CreatedAt);

                if (!_repository.SaveChanges())
                {
                    return StatusCode(500, new { message = "Internal Server error 500. Something went wrong" });
                }
                else
                {
                    return NoContent();
                }

            }

        }

        //DELETE 
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult DeleteProject(int id)
        {
            var projectModelfromRepo = _repository.GetProjectDataById(id);

            if (projectModelfromRepo == null)
            {
                _logger.LogError("The project with ID " + id + " doesn't exist.");

                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }
            else
            {
                _logger.LogInformation("Deleting project " + projectModelfromRepo.Name + " ");

                _repository.DeleteProject(projectModelfromRepo);
                if (!_repository.SaveChanges())
                {
                    return StatusCode(500, new { message = "Internal Server error 500. Something went wrong" });
                }
                else
                {
                    return NoContent();
                }

            }
        }

    }
}