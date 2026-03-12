using Nutrilife.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.DTO.Response
{
    public class SubscriptionResponse
    {
        public int SubscriptionId { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string NutritionistId { get; set; }
        public string NutritionistName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public SubscriptionStatus Status { get; set; }
        public string Notes { get; set; }
    }
}
