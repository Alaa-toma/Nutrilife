using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Models
{
    public class Nutritionist : ApplicationUser
    {
        public string Specialization { get; set; }
        public int YearsOfExperience { get; set; }
        public string Bio { get; set; }
        public decimal? ConsultationFee { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
    }
}
