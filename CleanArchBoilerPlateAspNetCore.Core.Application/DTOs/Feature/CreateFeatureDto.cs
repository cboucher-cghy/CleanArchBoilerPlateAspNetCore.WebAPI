using System.ComponentModel.DataAnnotations;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.Feature
{
    public class CreateFeatureDto
    {
        [Required]
        public string Name { get; set; }
    }
}
