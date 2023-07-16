using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Business.Interfaces
{
    public interface IAuthService
    {
        public Task<string> AuthenticateAsync(UserLoginDto userLogin);
        public Task<IdentityResult> RegisterAsync(UserRegistrationDto userRegistration);
    }
}