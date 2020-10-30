using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TAP.Models;

namespace TAP.Data
{
    public interface IProject
    {
        bool SaveChanges();
        IEnumerable<Project> GetAllProjects();
        Project GetProjectDataById(int id);
        void CreateProject(Project project);
        void UpdateProject(Project project,int id);
        void DeleteProject(Project project);
        ErrorDetails ErrorDescription(HttpStatusCode code);
    }
}
