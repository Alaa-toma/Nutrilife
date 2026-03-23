using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Models
{
    public class ApplicationUser : IdentityUser
    {
       
            public string? FullName { get; set; }
            public string? Gender { get; set; }
            public DateOnly DOF { get; set; }

            public string? codeResetPassword { get; set; }
            public DateTime? passwordResetCodeExpiry { get; set; }

           
        
    }
}
