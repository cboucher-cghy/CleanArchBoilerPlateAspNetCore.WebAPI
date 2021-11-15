using CleanArchBoilerPlateAspNetCore.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.FeatureValue
{
    public class FeatureValueDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int Sequence { get; set; }

        [Required]
        public FeatureStatus Status { get; set; }

        [MaxLength(30)]
        public string SAPCharacteristicValue300 { get; set; }

        [MaxLength(30)]
        public string SAPCharacteristicValueDescription300 { get; set; }

        [MaxLength(30)]
        public string SAPCharacteristicValue001 { get; set; }

        [MaxLength(30)]
        public string SAPCharacteristicValueDescription001 { get; set; }

        public string Comments { get; set; }

        public int FeatureId { get; set; }
    }
}
