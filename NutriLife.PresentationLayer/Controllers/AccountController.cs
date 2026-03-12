using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Nutrilife.LogicLayer.Service;
using Nutrilife.DataAccessLayer.DTO.Request;
using IAuthenticationService = Nutrilife.LogicLayer.Service.IAuthenticationService;
using RegisterRequest = Nutrilife.DataAccessLayer.DTO.Request.RegisterRequest;
using LoginRequest = Nutrilife.DataAccessLayer.DTO.Request.LoginRequest;

namespace NutriLife.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request);
            return Ok(result);
        }

        [HttpPost("register/nutritionist")]
        public async Task<IActionResult> RegisterNutritionist(NutritionistRequest request)
        {
            var result = await _authenticationService.RegisterNutritionistAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);


            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string UserId)
        {
            var isConfirmed = _authenticationService.ConfirmEmailAsync(token, UserId);

            if (isConfirmed.IsCompletedSuccessfully) { return Ok(); }
            return BadRequest();
        } 
    
    
    
    
    
    
    }
}
