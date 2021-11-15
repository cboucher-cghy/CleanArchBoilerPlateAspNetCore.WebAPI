using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public class ContextRole
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
