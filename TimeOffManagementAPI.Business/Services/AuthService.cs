using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Business.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    {
        _userService = userService;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<string> AuthenticateAsync(UserLogin userLogin)
    {
        await _signInManager.SignOutAsync();

        if (userLogin == null)
            throw new ArgumentNullException(nameof(userLogin)); // TODO exception mesajlarını değiştir

        if (string.IsNullOrWhiteSpace(userLogin.UserName))
            throw new ArgumentNullException(nameof(userLogin.UserName));

        if (string.IsNullOrWhiteSpace(userLogin.Password))
            throw new ArgumentNullException(nameof(userLogin.Password));

        var user = await _userService.GetByUsernameAsync(userLogin.UserName);

        var result = await _signInManager.PasswordSignInAsync(user, userLogin.Password, false, true);

        if (result.Succeeded)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, null);
        }
        else if (result.IsLockedOut)
        {
            var lockoutEndDate = await _userManager.GetLockoutEndDateAsync(user);
            var timeLeft = lockoutEndDate.Value.Subtract(DateTimeOffset.UtcNow).Minutes + 1;
            throw new ArgumentException($"Your account is locked out. Please try again {timeLeft} minutes later.");
        }
        else
        {
            await _userManager.AccessFailedAsync(user);
            throw new ArgumentException("Username or password is incorrect.");
        }

        return GenerateAccessToken(user);
    }

    public async Task<IdentityResult> RegisterAsync(UserRegistration userRegistration)
    {
        return await _userService.CreateAsync(userRegistration);
    }

    private string GenerateAccessToken(User user)
    {
        var key = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        var keyBytes = Encoding.UTF8.GetBytes(key);
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.Integer64)
        };

        _userManager.GetRolesAsync(user).Result.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

        var expires = DateTime.UtcNow.AddDays(1); // TODO sonra değiştir
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}