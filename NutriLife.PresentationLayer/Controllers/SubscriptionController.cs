using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.LogicLayer.Service;

namespace NutriLife.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        [Authorize(Roles = "Client")] // Client creates a subscription
        public async Task<IActionResult> Create(SubscriptionRequest request)
        {
            var result = await _subscriptionService.CreateAsync(request);
            return Ok(result);
        }


        [HttpGet("getbyid/{subscriptionId}")]
        public async Task<IActionResult> GetSubscriptionById(int subscriptionId)
        {
            var result = await _subscriptionService.GetByIdAsync(subscriptionId);
            return Ok(result);
        }

        // Nutritionist approves a subscription
        [HttpPut("approve/{subscriptionId}")]
        [Authorize(Roles = "Nutritionist")]
        public async Task<IActionResult> Approve(int subscriptionId)
        {
            var result = await _subscriptionService.ApproveAsync(subscriptionId);
            return Ok(result);
        }

        // Nutritionist rejects a subscription
        [HttpPut("reject/{subscriptionId}")]
        [Authorize(Roles = "Nutritionist")]
        public async Task<IActionResult> Reject(int subscriptionId)
        {
            var result = await _subscriptionService.RejectAsync(subscriptionId);
            return Ok(result);
        }


        // Client cancels a subscription
        [HttpPut("cancel/{subscriptionId}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Cancel(int subscriptionId)
        {
            var result = await _subscriptionService.CancelAsync(subscriptionId);
            return Ok(result);
        }

    }
}
