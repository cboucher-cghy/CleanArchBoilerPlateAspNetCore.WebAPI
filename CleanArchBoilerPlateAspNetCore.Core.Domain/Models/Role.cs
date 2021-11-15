using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public class Role
    {
        public Role()
        {
            Users = new List<User>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}
