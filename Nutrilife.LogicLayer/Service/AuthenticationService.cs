using Mapster;
using Microsoft.AspNetCore.Identity;
using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.DataAccessLayer.DTO.Response;
using Nutrilife.DataAccessLayer.Models;


namespace Nutrilife.LogicLayer.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IEmailSender _EmailSender;
        public AuthenticationService(UserManager<ApplicationUser> UserManager,
            IEmailSender emailSender) 
        {
            _UserManager = UserManager;
            _EmailSender = emailSender;
        }  
         public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _UserManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new RegisterResponse() { Success = false, Message = errors };
            }

            await _UserManager.AddToRoleAsync(user, "User");


            var token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);
            var EmailUrl = $"https://localhost:7217/api/Account/ConfirmEmail?token={token}";
            // للتاكد انه الي دخل على الصفحة وصلته رسالة عالايميل, مش حدت عشوائي استخدم الرابط

            await _EmailSender.SendEmailAsync(user.Email, "welcom", $"<h1> welcom {request.UserName} </h1>"+"  "
                + $"<a href='{EmailUrl}'> confirm </a> " );

            return new RegisterResponse() { Success = true, Message = "Success" };

        }


        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _UserManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new LoginResponse() { Success = false, Message = "Invalid Email" };
            }
            var result = await _UserManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                return new LoginResponse() { Success = false, Message = "Invalid Passwoed" };
            }

            return new LoginResponse() { Success = true, Message = "succcess" };

        }
    }
}
