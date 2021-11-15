using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public class User : AuditableEntity
    {
        public User()
        {
            Roles = new List<Role>();
            ContextRoleUsers = new List<ContextRoleUser>();
        }

        public string Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Departement { get; set; }

        public string JobTitle { get; set; }

        public List<Role> Roles { get; set; }

        public List<ContextRoleUser> ContextRoleUsers { get; set; }
    }
}
