using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Business.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetAllAsync();
        public Task<User> GetByIdAsync(int id);
        public Task<User> GetByUsernameAsync(string username);
        public Task<User> GetByEmailAsync(string email);
        public Task<User> CreateAsync(UserRegistrationDto user);
        public Task<User> UpdateAsync(User user);
        public Task<User> DeleteAsync(int id);
    }
}