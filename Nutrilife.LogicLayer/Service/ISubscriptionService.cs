using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.DataAccessLayer.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Service
{
    public interface ISubscriptionService
    {
        
        Task<SubscriptionResponse> CreateAsync(string clientId, SubscriptionRequest request);
        Task<List<SubscriptionResponse>> GetClientSubscriptionsAsync(string clientId);
        Task<List<SubscriptionResponse>> GetNutritionistSubscriptionsAsync(string nutritionistId);
        Task<SubscriptionResponse> CancelAsync(int subscriptionId, string clientId);
    }
    
}
