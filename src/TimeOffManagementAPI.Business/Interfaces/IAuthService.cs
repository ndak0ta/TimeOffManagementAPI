using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Business.Interfaces
{
    public interface IAuthService
    {
        public Task<LoginResponse> AuthenticateAsync(UserLogin userLogin);
        /* public Task<IdentityResult> RegisterAsync(UserRegistration userRegistration); */
    }
}