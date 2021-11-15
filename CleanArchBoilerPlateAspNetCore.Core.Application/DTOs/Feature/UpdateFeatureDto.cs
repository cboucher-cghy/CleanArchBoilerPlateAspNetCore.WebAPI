using CleanArchBoilerPlateAspNetCore.Core.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.Feature
{
    /// <summary>
    /// We cannot update the "CreatedBy" and "CreatedOn" properties, that is why we need to have an object without those.
    /// </summary>
    public class UpdateFeatureDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? Sequence { get; set; }

        [Required]
        public FeatureStatus Status { get; set; }

        [MaxLength(30)]
        public string SAPCharacteristicName300 { get; set; }

        [MaxLength(30)]
        public string SAPCharacteristicDescription300 { get; set; }

        public bool? IsSAPClass001Required { get; set; }

        [MaxLength(30)]
        public string SAPCharacteristicName001 { get; set; }

        [MaxLength(30)]
        public string SAPCharacteristicDescription001 { get; set; }

        public string Comments { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedOn { get; set; }
    }
}
