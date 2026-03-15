using Mapster;
using Microsoft.AspNetCore.Http;
using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.DataAccessLayer.DTO.Response;
using Nutrilife.DataAccessLayer.Models;
using Nutrilife.DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _SubscriptionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INutritionistRepository _nutritionistRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository,
            IHttpContextAccessor httpContextAccessor,
            INutritionistRepository nutritionistRepository)
        {
            _SubscriptionRepository = subscriptionRepository;
            _httpContextAccessor = httpContextAccessor;
            _nutritionistRepository = nutritionistRepository;
        }


        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<SubscriptionResponse> CreateAsync(SubscriptionRequest request)
        {
            // Validate nutritionist exists
            var nutritionist = await _nutritionistRepository
                .GetByIdAsync(request.NutritionistId);

            if (nutritionist == null)
                throw new Exception("Nutritionist not found");

            // Check client has no active subscription with same nutritionist
            var existing = await _SubscriptionRepository
                .GetActiveSubscriptionAsync(GetCurrentUserId(), request.NutritionistId);

            if (existing != null)
                throw new Exception("You already have an active subscription with this nutritionist");

            var subscription = request.Adapt<Subscription>();
            subscription.ClientId = GetCurrentUserId();
            subscription.Status = SubscriptionStatus.Pending;

            var created = await _SubscriptionRepository.CreateAsync(subscription);
            return created.Adapt<SubscriptionResponse>();
        }


        public async Task<SubscriptionResponse> ApproveAsync(int subscriptionId)
        {
            var subscription = await _SubscriptionRepository.GetByIdAsync(subscriptionId);

            if (subscription == null)
                throw new Exception("Subscription not found");

            // Only the nutritionist who owns it can approve
            if (subscription.NutritionistId != GetCurrentUserId())
                throw new UnauthorizedAccessException("You cannot approve this subscription");

            if (subscription.Status != SubscriptionStatus.Pending)
                throw new Exception("Only pending subscriptions can be approved");

            subscription.Status = SubscriptionStatus.Active;

            var updated = await _SubscriptionRepository.UpdateAsync(subscription);
            return updated.Adapt<SubscriptionResponse>();
        }



        public async Task<SubscriptionResponse> RejectAsync(int subscriptionId)
        {
            var subscription = await _SubscriptionRepository.GetByIdAsync(subscriptionId);

            if (subscription == null)
                throw new Exception("Subscription not found");

            if (subscription.NutritionistId != GetCurrentUserId())
                throw new UnauthorizedAccessException("You cannot reject this subscription");

            if (subscription.Status != SubscriptionStatus.Pending)
                throw new Exception("Only pending subscriptions can be rejected");

            subscription.Status = SubscriptionStatus.Cancelled;

            var updated = await _SubscriptionRepository.UpdateAsync(subscription);
            return updated.Adapt<SubscriptionResponse>();
        }


        public async Task<SubscriptionResponse?> GetByIdAsync(int subscriptionId)
        {
            var subscription = await _SubscriptionRepository
                .GetByIdAsync(subscriptionId);

            if (subscription == null)
                throw new Exception("Subscription not found in db!!!!!!!!!");

            // Verify current user owns this subscription
            var currentUserId = GetCurrentUserId();
            if (subscription.ClientId != currentUserId &&
                subscription.NutritionistId != currentUserId)
                throw new UnauthorizedAccessException("You do not have access to this subscription");

            return subscription.Adapt<SubscriptionResponse>();
        }

        public async Task<SubscriptionResponse> CancelAsync(int subscriptionId)
        {
            var subscription = await _SubscriptionRepository.GetByIdAsync(subscriptionId);

            if (subscription == null)
                throw new Exception("Subscription not found");

            if (subscription.ClientId != GetCurrentUserId())
                throw new UnauthorizedAccessException("You cannot cancel this subscription");

            if (subscription.Status == SubscriptionStatus.Cancelled)
                throw new Exception("Subscription is already cancelled");

            if (subscription.Status == SubscriptionStatus.Finished)
                throw new Exception("Cannot cancel an expired subscription");

            subscription.Status = SubscriptionStatus.Cancelled;

            var updated = await _SubscriptionRepository.UpdateAsync(subscription);
            return updated.Adapt<SubscriptionResponse>();
        }

    }
}
