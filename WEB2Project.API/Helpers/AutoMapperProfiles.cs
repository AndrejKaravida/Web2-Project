using AutoMapper;
using WEB2Project.API.Dtos;
using WEB2Project.API.Models;
using WEB2Project.Dtos;

namespace WEB2Project.Helpers
{
    public class AutoMapperProfiles : Profile
    {
       public AutoMapperProfiles()
       {
            CreateMap<User, UserForDetailedDto>();
            CreateMap<UserForRegisterDto, User>();

        }
    }
}
