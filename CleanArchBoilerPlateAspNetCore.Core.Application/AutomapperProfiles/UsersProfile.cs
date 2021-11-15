using AutoMapper;
using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.User;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Models;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.AutomapperProfiles
{
    class UsersProfile : Profile
    {
        public UsersProfile()
        {
            // https://docs.automapper.org/en/latest/Mapping-inheritance.html
            CreateMap<AuditableEntity, UserDto>().ReverseMap();


            CreateMap<User, UserDto>().ReverseMap();

        }
    }
}
