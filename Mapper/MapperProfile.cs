using AuthenticationService.DTOs;
using AuthenticationService.Models;
using AutoMapper;

namespace AuthenticationService.Mapper
{
    public class MapperProfile : Profile
    {


        public MapperProfile()
        {
            CreateMap<RegisterDTO, User>();

        }
    }
}
