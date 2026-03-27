using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.DTO.Response
{
    public class ClientResponse
    {
        public string Client_id { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Gender { get; set; }
        public DateOnly DOF { get; set; }
        public string? PhoneNumber { get; set; }
        public float Height { get; set; }
        public string? Disease { get; set; }
        public string? Goal { get; set; }
    }
}
