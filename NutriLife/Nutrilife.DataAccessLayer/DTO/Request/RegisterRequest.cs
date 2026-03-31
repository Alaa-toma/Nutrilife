using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.DTO.Request
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(8)]
        public string Password { get; set; } = null!;
        [Required]
        public string? Gender { get; set; }
        public DateOnly DOF { get; set; }        // Date of Birth

        [Phone]
        public string? PhoneNumber { get; set; }

       
    }
}
