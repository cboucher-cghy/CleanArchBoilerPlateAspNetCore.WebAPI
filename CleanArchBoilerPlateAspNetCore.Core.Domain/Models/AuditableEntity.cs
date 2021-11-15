using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public abstract class AuditableEntity
    {

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string LastModifiedBy { get; set; }

        [Required]
        public DateTime LastModifiedOn { get; set; }
    }
}
