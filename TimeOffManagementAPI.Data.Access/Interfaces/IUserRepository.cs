using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Data.Access.Interfaces;

public interface IUserRepository
{
    public Task<IEnumerable<User>> GetAllAsync();
    public Task<User> GetByIdAsync(int id);
    public Task<User> GetByUsernameAsync(string username);
    public Task<User> GetByEmailAsync(string email);
    public Task<User> CreateAsync(User user);
    public Task<User> UpdateAsync(User user);
    public Task<User> DeleteAsync(User user);
}
