using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public class Tag : AuditableEntity
    {
        public Tag()
        {
            FeatureValues = new List<FeatureValue>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        public List<FeatureValue> FeatureValues { get; set; }
    }
}
