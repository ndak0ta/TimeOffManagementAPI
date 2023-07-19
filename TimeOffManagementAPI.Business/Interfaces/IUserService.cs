using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;


namespace TimeOffManagementAPI.Business.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetAllAsync();
        public Task<User> GetByIdAsync(string id);
        public Task<User> GetByUsernameAsync(string username);
        public Task<User> GetByEmailAsync(string email);
        public Task<IdentityResult> CreateAsync(UserRegistration user);
        public Task<IdentityResult> UpdateAsync(UserUpdate user);
        public Task<IdentityResult> DeleteAsync(string id);
        public Task<IdentityResult> ChangePasswordAsync(UserChangePassword user);
        public Task<int> TimeOffLeftAsync(string userId);
    }
}