using CleanArchBoilerPlateAspNetCore.Core.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public class FeatureValue : AuditableEntity
    {
        public FeatureValue()
        {
            FeatureGroups = new List<FeatureGroup>();
            Tags = new List<Tag>();
            Contexts = new List<Context>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? Sequence { get; set; }

        [Required]
        public FeatureStatus Status { get; set; }

        public string Comments { get; set; }

        public int FeatureId { get; set; }

        [ForeignKey("FeatureId")]
        public Feature Feature { get; set; }

        public List<FeatureGroup> FeatureGroups { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Context> Contexts { get; set; }
    }
}
