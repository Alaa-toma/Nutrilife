using Nutrilife.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.DTO.Request
{
    public class AppointmentRequest
    {
        
        [Required]
        public int SubscriptioId { get; set; }

        [Required]
        public DateTime appointment_date { get; set; }

        [Required]
        public AppointmentType type { get; set; }
        public string? Notes { get; set; }

    }
}
