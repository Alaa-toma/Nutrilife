using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Models
{
    public class Appointment : AuditableEntity
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int SubscriptioId { get; set; }
        public Subscription Subscription{ get; set; }

        [Required]
        public DateTime appointment_date {  get; set; }

        public AppointmentStatus status { get; set; }

        [Required]
        public AppointmentType type { get; set; }
        public string? Notes { get; set; }

    }
}
