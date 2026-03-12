using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Models
{
    public class Client : ApplicationUser
    {

        public float Height { get; set; }
        public float Weight { get; set; }
        public string? Disease { get; set; }
        public string? Goal { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }
    }
}
