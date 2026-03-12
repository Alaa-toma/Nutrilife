using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.DataAccessLayer.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        Task<SubscriptionResponse> ISubscriptionService.CancelAsync(int subscriptionId, string clientId)
        {
            throw new NotImplementedException();
        }

        Task<SubscriptionResponse> ISubscriptionService.CreateAsync(string clientId, SubscriptionRequest request)
        {
            throw new NotImplementedException();
        }

        Task<List<SubscriptionResponse>> ISubscriptionService.GetClientSubscriptionsAsync(string clientId)
        {
            throw new NotImplementedException();
        }

        Task<List<SubscriptionResponse>> ISubscriptionService.GetNutritionistSubscriptionsAsync(string nutritionistId)
        {
            throw new NotImplementedException();
        }
    }
}
