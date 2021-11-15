using System.ComponentModel.DataAnnotations;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.DTOs.User
{
    public class UserDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Departement { get; set; }

        public string JobTitle { get; set; }
    }
}
