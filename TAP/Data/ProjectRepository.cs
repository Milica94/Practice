using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TAP.Models;

namespace TAP.Data
{
    public class ProjectRepository : IProject
    {
        private readonly TAPContext _context;
        public ProjectRepository(TAPContext context)
        {
            _context = context;
        }
        public void CreateProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            _context.Project.Add(project);
        }

        public void DeleteProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            _context.Project.Remove(project);
        }

        public ErrorDetails ErrorDescription(HttpStatusCode code)
        {
            var _httpStatusCode = (int)code;

            if (_httpStatusCode == 400)
            {
                return new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Bad request"
                };

            }
            else if (_httpStatusCode == 401)
            {
                return new ErrorDetails()
                {
                    StatusCode = 401,
                    Message = "You are unauthorized user. You have no access. Please contact your authorization authority."
                };
            }
            else if (_httpStatusCode == 403)
            {
                return new ErrorDetails()
                {
                    StatusCode = 403,
                    Message = "Forbidden"
                };
            }
            else if (_httpStatusCode == 404)
            {
                return new ErrorDetails()
                {
                    StatusCode = 404,
                    Message = "Not found"
                };
            }
            else
            {
                return new ErrorDetails()
                {
                    StatusCode = 500,
                    Message = "Internal Server error. Something went wrong"
                };
            }
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return _context.Project.ToList();
        }

        public Project GetProjectDataById(int id)
        {
            return _context.Project.Where(projectId => projectId.Id == id).FirstOrDefault();
        }

        public bool SaveChanges()
        {
            try
            {
                return (_context.SaveChanges() >= 0);
            }
            catch (UpdateException update)
            {
                Debug.WriteLine(update.InnerException);
                return false;

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException excp)
            {
                Debug.WriteLine(excp.InnerException);
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return false;
            }
        }

        public void UpdateProject(Project project,int id)
        {
            
                var new_project = _context.Project.Find(id);
                new_project.Name = project.Name;
                new_project.CreatedAt = project.CreatedAt;
                new_project.Status = project.Status;
                new_project.ClientId = project.ClientId;
                _context.Entry(new_project).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            

            try
            {
                _context.SaveChanges();
            }

            catch (UpdateException update)
            {
                Debug.WriteLine(update.InnerException);

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException excp)
            {
                Debug.WriteLine(excp.InnerException);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
            }
        }
    }
}
