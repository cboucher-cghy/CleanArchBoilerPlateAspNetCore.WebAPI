using CleanArchBoilerPlateAspNetCore.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public class Context : AuditableEntity
    {
        public Context()
        {
            FeaturesAllocations = new List<FeatureAllocation>();
            CoreFeatures = new List<FeatureValue>();
            ContextRoleUsers = new List<ContextRoleUser>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int Version { get; set; }

        public int? ContextId { get; set; }

        [ForeignKey("ContextId")]
        public Context PreviousContext { get; set; }

        public DateTime? PublicAnnouncementDate { get; set; }

        public DateTime? SalesOrderStartDate { get; set; }

        public DateTime? SalesOrderEndDate { get; set; }

        public DateTime? ProductionStartDate { get; set; }

        [Required]
        public ContextSatus Status { get; set; }

        public List<FeatureAllocation> FeaturesAllocations { get; set; }

        public List<FeatureValue> CoreFeatures { get; set; }

        public List<ContextRoleUser> ContextRoleUsers { get; set; }
    }
}
