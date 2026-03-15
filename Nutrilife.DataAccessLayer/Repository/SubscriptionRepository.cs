using Microsoft.EntityFrameworkCore;
using Nutrilife.DataAccessLayer.Data;
using Nutrilife.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Repository
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;
        public SubscriptionRepository(ApplicationDbContext context):base(context) {
            _context = context;
        }



        public async Task<Subscription?> GetActiveSubscriptionAsync(string clientId, string nutritionistId)
        {
            return await GetOne(
         filter: s => s.ClientId == clientId &&
                      s.NutritionistId == nutritionistId &&
                      (s.Status == SubscriptionStatus.Pending ||
                       s.Status == SubscriptionStatus.Active)
     );
        }


        public async Task<Subscription?> GetByIdAsync(int subscriptionId)
        {
            return await GetOne(
                filter: s => s.SubscriptionId == subscriptionId,
                includes: new[] { "Client", "Nutritionist" }
            );
        }

        public async Task<Subscription> UpdateAsync(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }
    }
}
