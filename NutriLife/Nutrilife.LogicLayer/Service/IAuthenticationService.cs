using Microsoft.AspNetCore.Identity;
using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.DataAccessLayer.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Service
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(ClientRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<RegisterResponse> RegisterNutritionistAsync(NutritionistRequest request);
        Task<bool> ConfirmEmailAsync(string token, string UserId);
        Task<bool> ResendConfirmationEmailAsync(string email);
        Task<ResetPasswordResponse> resetPasswordAsync(ResendConfirmationEmailDTO request);
        Task<ResetPasswordResponse> NewPasswordAsync(NewPasswordRequest request);

        Task<List<ClientResponse>> GetAllClientsInNutrilife();
         Task<List<NutritionistResponse>> GetAllNutritionistInNutrilife();
        Task<DeleteAccountResponse> DeleteAccountAsync(DeleteAccountRequest request);
    }
}
