using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.DTO.Request
{
    public class ClientRequest : RegisterRequest
    {

        public float Height { get; set; }
        public float Weight { get; set; }

        public string? Disease { get; set; }
        public string? Goal { get; set; }
    }
}
