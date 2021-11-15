using AutoMapper;
using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.Feature;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Models;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.AutomapperProfiles
{
    public class FeaturesProfile : Profile
    {
        public FeaturesProfile()
        {
            // https://docs.automapper.org/en/latest/Mapping-inheritance.html
            CreateMap<AuditableEntity, FeatureDto>().ReverseMap();

            // Also required for subclass.
            CreateMap<Feature, FeatureDto>()
            //.ForMember(
            //    p => p.Status,
            //    opt => opt.MapFrom(x =>
            //        x.Status.Description()))
            .ReverseMap();

            CreateMap<Feature, CreateFeatureDto>().ReverseMap();
            CreateMap<Feature, UpdateFeatureDto>().ReverseMap();
        }
    }
}
