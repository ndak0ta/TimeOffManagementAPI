using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;


namespace TimeOffManagementAPI.Business.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<UserInfo>> GetAllAsync();
        public Task<IEnumerable<User>> GetAllPasiveAsync();
        public Task<User> GetByIdAsync(string id);
        public Task<User> GetByUsernameAsync(string username);
        public Task<User> GetByEmailAsync(string email);
        public Task<UserInfo> GetUserInfoAsync(string userId);
        public Task<IdentityResult> CreateAsync(UserRegistration user);
        public Task<UserUpdate> UpdateAsync(UserUpdate userUpdate);
        public Task<IdentityResult> UpdateContactAsync(UserUpdateContact user);
        public Task<IdentityResult> DeleteAsync(string id);
        public Task<IdentityResult> AddUserToRoleAsync(string userId, string role);
        public Task<IdentityResult> ChangePasswordAsync(UserChangePassword user);
        public Task<IdentityResult> UpdateRemaningAnnualTimeOff(string userId);
        public Task UpdateAnnualTimeOffAsync();
        public Task<IdentityResult> SetAnnualTimeOffAsync(string userId, int annualTimeOff);
        public Task<Role> GetRoleAsync(string userId);
    }
}