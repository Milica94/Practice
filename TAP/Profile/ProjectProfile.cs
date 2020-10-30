using TAP.Dtos;
using TAP.Models;

namespace TAP.Profile
{
    public class ProjectProfile : AutoMapper.Profile
    {
        public ProjectProfile()
        {
            //Source -> Target
            CreateMap<Project, ProjectReadDTO>(); //get
            CreateMap<ProjectCreateDTO, Project>(); //post
            CreateMap<ProjectUpdateDTO, Project>(); //put
            CreateMap<Project, ProjectUpdateDTO>();
        }
    }
}
