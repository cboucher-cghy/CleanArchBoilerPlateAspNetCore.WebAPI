using CleanArchBoilerPlateAspNetCore.Core.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public class Feature : AuditableEntity
    {
        public Feature()
        {
            FeatureValues = new List<FeatureValue>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? Sequence { get; set; }

        [Required]
        public FeatureStatus Status { get; set; }

        public string Comments { get; set; }

        public List<FeatureValue> FeatureValues { get; set; }
    }
}
