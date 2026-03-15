using Nutrilife.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.Repository
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<Subscription?> GetActiveSubscriptionAsync(string clientId, string nutritionistId);

        Task<Subscription?> GetByIdAsync(int subscriptionId); // return subscription
        Task<Subscription> UpdateAsync(Subscription subscription);
    }
}
