using TAP.Dtos;
using TAP.Models;

namespace TAP
{
    public class ClientProfile : AutoMapper.Profile
    {
        public ClientProfile()
        {
            //Source -> Target
            CreateMap<Client, ClientReadDto>();//get
            CreateMap<ClientCreateDto, Client>();//post
            CreateMap<ClientUpdateDto, Client>();//put
            CreateMap<Client, ClientUpdateDto>();//delete
        }
    }
}
