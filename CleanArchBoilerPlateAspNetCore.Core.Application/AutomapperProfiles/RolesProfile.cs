using AutoMapper;
using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.Role;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Models;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.AutomapperProfiles
{
    class RolesProfile : Profile
    {
        public RolesProfile()
        {
            // https://docs.automapper.org/en/latest/Mapping-inheritance.html
            CreateMap<AuditableEntity, RoleDto>().ReverseMap();


            CreateMap<Role, RoleDto>().ReverseMap();

        }
    }
}
