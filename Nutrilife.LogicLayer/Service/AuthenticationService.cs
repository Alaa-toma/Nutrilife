using Mapster;
using Microsoft.AspNetCore.Identity;
using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.DataAccessLayer.DTO.Response;
using Nutrilife.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        public AuthenticationService(UserManager<ApplicationUser> UserManager) 
        {
            _UserManager = UserManager;
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
            return new RegisterResponse() { Success = true, Message = "Success" };

        }
    }
}
