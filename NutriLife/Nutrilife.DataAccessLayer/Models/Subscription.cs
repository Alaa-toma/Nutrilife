using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }

        // Client FK
        public string ClientId { get; set; }
        public Client Client { get; set; }

        // Nutritionist FK
        public string NutritionistId { get; set; }
        public Nutritionist Nutritionist { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public SubscriptionStatus Status { get; set; }
        public decimal Price { get; set; }
        public string? Notes { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
