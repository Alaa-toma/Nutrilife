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
        
        Task<SubscriptionResponse> CreateAsync( SubscriptionRequest request); // create subscription request 
        Task<SubscriptionResponse> ApproveAsync(int subscriptionId); // accepted
        Task<SubscriptionResponse> RejectAsync(int subscriptionId); // not accepted 
        Task<SubscriptionResponse?> GetByIdAsync(int subscriptionId);
        Task<List<SubscriptionResponse>> GetClientsByNutritionistAsync();
        Task<SubscriptionResponse> CancelAsync(int subscriptionId);
    }
    
}
