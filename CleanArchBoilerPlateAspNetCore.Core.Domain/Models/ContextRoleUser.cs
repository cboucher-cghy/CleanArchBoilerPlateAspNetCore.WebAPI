using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public class ContextRoleUser
    {
        public int Id { get; set; }

        [Required]
        public int ContextId { get; set; }

        [ForeignKey("ContextId")]
        public Context Context { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public ContextRole ContextRole { get; set; }
    }
}
