using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Interfaces
{
    public interface IAuthService
    {
        public Task<User> AuthenticateAsync(User user);
        public Task<User> RegisterAsync(User user);
    }
}