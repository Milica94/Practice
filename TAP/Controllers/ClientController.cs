using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

    public class ClientController : ControllerBase
    {

        private readonly IClient _repository;
        private readonly ILogger<ClientController> _logger;
        private readonly IMapper _mapper;
        
        public ClientController(ILogger<ClientController> logger, IClient repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
  

        //GET V3 allow anonymus
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<ClientReadDto>> GetAllClients()
        {

            var clientItems = _repository.GetAllClients();

            if (clientItems != null)
            {
                _logger.LogInformation("Getting " + clientItems.Count() + " clients from database.");

                return Ok(_mapper.Map<IEnumerable<ClientReadDto>>(clientItems));
            }
            else
            {
                _logger.LogError("Can't find any client in database.");

                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }
        }

        //GET V2 handle authorization error
        [AllowAnonymous]
        [ApiVersion("2.0")]
        [HttpGet]
        public ActionResult<IEnumerable<ClientReadDto>> GetAllClientsV2()
        {
            var clientItems = _repository.GetAllClients();

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(_repository.ErrorDescription(HttpStatusCode.Unauthorized));
            }           

            if (clientItems != null)
            {
                _logger.LogInformation("Getting " + clientItems.Count() + " clients from database.");

                return Ok(_mapper.Map<IEnumerable<ClientReadDto>>(clientItems));
            }
            else
            { 
                _logger.LogError("Can't find any client in database.");

                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }

        }

        //GET_BY_ID
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<ClientReadDto> GetClientById(int id)
        {
            var clientItem = _repository.GetClientDataById(id);

            if (clientItem != null)
            {
                _logger.LogInformation("Reading client " + clientItem.Name + ".");

                return Ok(_mapper.Map<ClientReadDto>(clientItem));
            }
            else
            {
                _logger.LogError("Client with ID " +id+" doesn't exist");


                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }
        }

        //POST 
        [Authorize]
        [HttpPost]
        public ActionResult<ClientReadDto> PostClient(ClientCreateDto client)
        {
          
            if (!ModelState.IsValid)
            {
                _logger.LogError("Bad Request 400."); //ModelState

                return BadRequest(ModelState);
            }

            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Unauthorized(_repository.ErrorDescription(HttpStatusCode.Unauthorized));

            //}

            if (client != null)
            {
                var clientModel = _mapper.Map<Client>(client);
                _repository.CreateClient(clientModel);

                if (_repository.SaveChanges())
                {
                    var clientDto = _mapper.Map<ClientReadDto>(clientModel);

                    _logger.LogInformation("Creating client" + clientDto.Name + ".");

                    return CreatedAtRoute(new {Id = clientDto.Id}, clientDto);
                }
                else
                {
                    return StatusCode(500, new { message = "Internal Server error 500. Something went wrong" });

                }

            } 
            else
            {
                _logger.LogError("Client "+client.Name+ "can't be created.");

                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }
           
        }

        //PUT 
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult UpdateClient(int id, ClientUpdateDto clientUpdateDto)
        {
            var clientModelfromRepo = _repository.GetClientDataById(id);
            if (clientModelfromRepo == null)
            {
                _logger.LogError("The client with ID: " + id + " doesn't exist.");
                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }
            else
            {
                _mapper.Map(clientUpdateDto, clientModelfromRepo);

                _logger.LogInformation("Updating client " + clientModelfromRepo.Name + ".");

                _repository.UpdateClient(clientModelfromRepo);

                if (!_repository.SaveChanges())
                {
                    return StatusCode(500, new { message = "Internal Server error 500." + "Something went wrong" });
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
        public ActionResult PartialClientUpdate(int id, JsonPatchDocument<ClientUpdateDto> patchDoc)
        {
            var clientModelfromRepo = _repository.GetClientDataById(id);

            if (clientModelfromRepo == null)
            {
                _logger.LogError("The client with ID " + id + " doesn't exist.");

                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }
            else
            {
                var clientToPatch = _mapper.Map<ClientUpdateDto>(clientModelfromRepo);

                patchDoc.ApplyTo(clientToPatch, ModelState);

                if (!TryValidateModel(clientToPatch))
                {
                    return ValidationProblem(ModelState);
                }

                _mapper.Map(clientToPatch, clientModelfromRepo);

                _repository.UpdateClient(clientModelfromRepo);

                _logger.LogInformation("Updated client's property.\n New Value: "
                   + clientModelfromRepo.Id + " "
                   + clientModelfromRepo.Name + " "
                   + clientModelfromRepo.CreatedAt);

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
        public ActionResult DeleteClient(int id)
        {
            var clientModelfromRepo = _repository.GetClientDataById(id);

            if (clientModelfromRepo == null)
            {
                _logger.LogError("The client with ID " + id + " doesn't exist.");

                return NotFound(_repository.ErrorDescription(HttpStatusCode.NotFound));
            }
            else
            {
                _logger.LogInformation("Deleting client " + clientModelfromRepo.Name + " ");

                _repository.DeleteClient(clientModelfromRepo);
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

  

