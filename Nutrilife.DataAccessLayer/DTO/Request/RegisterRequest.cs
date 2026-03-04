using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.DTO.Request
{
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
