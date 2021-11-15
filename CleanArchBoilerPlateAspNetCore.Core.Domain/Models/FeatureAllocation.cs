using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public class FeatureAllocation
    {
        public int Id { get; set; }

        [Required]
        public int FeatureValueId { get; set; }

        [ForeignKey("FeatureValueId")]
        public FeatureValue FeatureValue { get; set; }


        [Required]
        public int ContextId { get; set; }

        [ForeignKey("ContextId")]
        public Context Context { get; set; }

        public DateTime EffectivityStartDate { get; set; }

        public DateTime EffectivityEndDate { get; set; }
    }
}
