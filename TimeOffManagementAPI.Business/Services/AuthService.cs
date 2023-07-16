using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Business.Services;

public class AuthService : IAuthService
{
    private readonly UserService _userService;
    private readonly UserManager<User> _userManager;

    public AuthService(UserService userService, UserManager<User> userManager)
    {
        _userService = userService;
        _userManager = userManager;
    }

    public async Task<string> AuthenticateAsync(UserLoginDto userLogin)
    {
        if (userLogin == null)
            throw new ArgumentNullException(nameof(userLogin)); // TODO exception mesajlarını değiştir

        if (string.IsNullOrWhiteSpace(userLogin.Username))
            throw new ArgumentNullException(nameof(userLogin.Username));

        if (string.IsNullOrWhiteSpace(userLogin.Password))
            throw new ArgumentNullException(nameof(userLogin.Password));

        var user = await _userManager.FindByNameAsync(userLogin.Username);

        if (user == null || await _userManager.CheckPasswordAsync(user, userLogin.Password))
            throw new ArgumentException("Username or password is incorrect.");

        return await _userManager.GenerateUserTokenAsync(user, "Default", "Login");
    }

    public async Task<IdentityResult> RegisterAsync(UserRegistrationDto userRegistration)
    {
        return await _userService.CreateAsync(userRegistration); // TODO  Identity'nin kendi metodlarını kullan
    }
}