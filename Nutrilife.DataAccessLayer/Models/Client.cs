using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateOnly DOF { get; set; } // Date Of Birth
        public string Phone { get; set; } = null!;
        public float Height { get; set; }   
        public string Disease { get; set; } = null!;
        public string Goal { get; set; } = null!;


    }
}
