using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Services;

public class AuthService : IAuthService
{
    public Task<User> AuthenticateAsync(User user)
    {
        return Task.FromResult(user);   
    }

    public Task<User> RegisterAsync(User user)
    {
        return Task.FromResult(user);
    }
}