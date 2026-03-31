using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.DTO.Response
{
    public class AppointmentResponse
    {
        public bool Confirmd { get; set; }
        public string message { get; set; }
        public int AppointmentId { get; set; }
        public String MeetingLink  { get; set; }

    }
}
