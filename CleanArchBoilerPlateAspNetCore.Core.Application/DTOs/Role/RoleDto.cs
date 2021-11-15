using System.ComponentModel.DataAnnotations;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.Role
{
    public class RoleDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
