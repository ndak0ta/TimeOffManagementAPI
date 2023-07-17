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

    public async Task<string> AuthenticateAsync(UserLoginDto userLogin)
    {
        if (userLogin == null)
            throw new ArgumentNullException(nameof(userLogin)); // TODO exception mesajlarını değiştir

        if (string.IsNullOrWhiteSpace(userLogin.Username))
            throw new ArgumentNullException(nameof(userLogin.Username));

        if (string.IsNullOrWhiteSpace(userLogin.Password))
            throw new ArgumentNullException(nameof(userLogin.Password));

        var result = await _signInManager.PasswordSignInAsync(userLogin.Username, userLogin.Password, false, false);

        if (!result.Succeeded)
            throw new ArgumentException("Username or password is incorrect.");

        var user = await _userService.GetByUsernameAsync(userLogin.Username);

        return GenerateAccessToken(user);
    }

    public async Task<IdentityResult> RegisterAsync(UserRegistrationDto userRegistration)
    {
        return await _userService.CreateAsync(userRegistration); // TODO  Identity'nin kendi metodlarını kullan
    }

    private string GenerateAccessToken(User user)
    {
        var key = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        var keyBytes = Encoding.UTF8.GetBytes(key);
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.Integer64)
        };

        _userManager.GetRolesAsync(user).Result.ToList().ForEach(role => claims.Append(new Claim(ClaimTypes.Role, role)));

        var expires = DateTime.UtcNow.AddMinutes(5);
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