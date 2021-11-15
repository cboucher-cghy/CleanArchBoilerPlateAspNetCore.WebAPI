using CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.FeatureValue;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.Feature
{
    class FeatureLibraryDto
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
        public string SAPCharacteristicName300 { get; set; }

        [MaxLength(30)]
        public string SAPCharacteristicDescription300 { get; set; }

        public bool? IsSAPClass001Required { get; set; }

        [MaxLength(30)]
        public string SAPCharacteristicName001 { get; set; }

        [MaxLength(30)]
        public string SAPCharacteristicDescription001 { get; set; }

        public string Comments { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedOn { get; set; }

        public List<FeatureValueDto> FeatureValues { get; set; }
    }
}
