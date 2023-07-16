using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;


namespace TimeOffManagementAPI.Business.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetAllAsync();
        public Task<IEnumerable<User>> GetAllActiveAsync();
        public Task<User> GetByIdAsync(string id);
        public Task<User> GetByUsernameAsync(string username);
        public Task<User> GetByEmailAsync(string email);
        public Task<IdentityResult> CreateAsync(UserRegistrationDto user);
        public Task<IdentityResult> UpdateAsync(User user);
        public Task<IdentityResult> DeleteAsync(string id);
    }
}