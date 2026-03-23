using Azure.Core;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.DataAccessLayer.DTO.Response;
using Nutrilife.DataAccessLayer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Nutrilife.LogicLayer.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IEmailSender _EmailSender;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(UserManager<ApplicationUser> UserManager,
            IEmailSender emailSender,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
        {
            _UserManager = UserManager;
            _EmailSender = emailSender;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }  
         public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<Client>();
            var result = await _UserManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new RegisterResponse() { Success = false, Message = errors };
            }

            await _UserManager.AddToRoleAsync(user, "Client");


            var token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);

            var EmailUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Account/ConfirmEmail?token={token}&userid={user.Id}";
            // للتاكد انه الي دخل على الصفحة وصلته رسالة عالايميل, مش حدت عشوائي استخدم الرابط

            await _EmailSender.SendEmailAsync(user.Email, "welcom", $"<h1> welcom {request.UserName} </h1>"+"  "
                + $"<a href='{EmailUrl}'> confirm </a> " );

            return new RegisterResponse() { Success = true, Message = "Success" };

        }

        public async Task<RegisterResponse> RegisterNutritionistAsync(NutritionistRequest request)
        {
            var user = request.Adapt<Nutritionist>();

            var result = await _UserManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return new RegisterResponse { Success = false, Message = result.Errors.First().Description };

            await _UserManager.AddToRoleAsync(user, "Nutritionist");

            var token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);
            var EmailUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Account/ConfirmEmail?token={token}&userid={user.Id}";
            // للتاكد انه الي دخل على الصفحة وصلته رسالة عالايميل, مش حدت عشوائي استخدم الرابط

            await _EmailSender.SendEmailAsync(user.Email, "welcom", $"<h1> welcom {request.UserName} </h1>" + "  "
                + $"<a href='{EmailUrl}'> confirm </a> ");

            return new RegisterResponse() { Success = true, Message = "Success" };

        }


        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _UserManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new LoginResponse() { Success = false, Message = "Invalid Email" };
            }

            if (! await _UserManager.IsEmailConfirmedAsync(user))
            {
                return new LoginResponse() { Success = false, Message = "Email is not confirmed" };
            }

            var result = await _UserManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                return new LoginResponse() { Success = false, Message = "Invalid Passwoed" };
            }

            return new LoginResponse() { Success = true, Message = "succcess", AccessToken = await GenerateAccessToken(user)};

        }

        public async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var roles = await _UserManager.GetRolesAsync(user);
            var UserClaims = new List<Claim>()
            { // المعلومات الي باخذها لما افك تشفير التوكن
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
       issuer: _configuration["Jwt:Issuer"],
       audience: _configuration["Jwt:Audience"],
       claims: UserClaims,
       expires: DateTime.Now.AddDays(5),
       signingCredentials: credentials
       );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ConfirmEmailAsync(string token, string UserId)
        {
            var user = await _UserManager.FindByIdAsync(UserId);
            if (user == null) {return false;}

            var result = await _UserManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            return true;

        }

        public async Task<bool> ResendConfirmationEmailAsync(string email)
        {
            var user = await _UserManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("User not found");

            if (user.EmailConfirmed)
                throw new Exception("Email is already confirmed");

            var token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);


            var EmailUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Account/ConfirmEmail?token={token}&userid={user.Id}";

            await _EmailSender.SendEmailAsync(user.Email, "welcom", $"<h1> welcom {user.UserName} </h1>" + "  "
                 + $"<a href='{EmailUrl}'> confirm </a> ");

            return true;
        }


        public async Task<ResetPasswordResponse> resetPasswordAsync(ResendConfirmationEmailDTO request)
        {
            var user = await _UserManager.FindByEmailAsync(request.Email);
            if(user == null) // نتأكد انه اليوزر موجود
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message="email not found"
                };
            }

            var random = new Random();
            var code = random.Next(1000, 9999).ToString(); // كود عشوائي من 4 ارقام

            user.codeResetPassword = code;
            user.passwordResetCodeExpiry = DateTime.UtcNow.AddMinutes(15); //  الكود صالح لمدة 15 دقيقة

            await _UserManager.UpdateAsync(user);

            await _EmailSender.SendEmailAsync(request.Email, "reset Password", $"<p> code is {code} </p>");

            return new ResetPasswordResponse()
            {
                Success = true,
                Message = "Code Sent To Your Email"
            };
        }

        public async Task<ResetPasswordResponse> NewPasswordAsync(NewPasswordRequest request)
        {
            var user = await _UserManager.FindByEmailAsync(request.Email);
            if (user == null) // نتأكد انه اليوزر موجود
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "email not found"
                };
            }
            else if(user.codeResetPassword != request.Code)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "Invalid Code"
                };
            }
            else if(user.passwordResetCodeExpiry < DateTime.UtcNow)
            {
                    return new ResetPasswordResponse()
                    {
                        Success = false,
                        Message = "Code Expired"
                    };
            }

            var isSmaePass = await _UserManager.CheckPasswordAsync(user, request.NewPassword);
            if (isSmaePass)
            {
                    return new ResetPasswordResponse()
                    {
                        Success = false,
                        Message = "This is your old Pass! The new must be different."
                    };
            }

            var token = await _UserManager.GeneratePasswordResetTokenAsync(user); //ما الها فائدة هنا, لانه الطريقة ما تطلب توكن, الاستخدام حتى يروح الايرور في ميثود الابديت
           var result=  await _UserManager.ResetPasswordAsync(user,token, request.NewPassword);
            if (!result.Succeeded)
            {
                    return new ResetPasswordResponse()
                    {
                        Success = false,
                        Message = "Password reset Faild!"
                    };
            }

            await _EmailSender.SendEmailAsync(request.Email, "Change Password", "$<p> Your Password Changed Successfully.. </p>");


             return new ResetPasswordResponse()
                {
                    Success = true,
                    Message = "Changed Successfully.."
                };
        }
    }
}
