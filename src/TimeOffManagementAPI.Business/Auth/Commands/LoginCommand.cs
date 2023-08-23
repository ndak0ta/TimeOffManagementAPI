using AutoMapper;
using MediatR;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace TimeOffManagementAPI.Business.Auth.Commands;

public record LoginCommand : IRequest<LoginResponse>
{
    public LoginCommand(UserLogin userLogin)
    {
        UserLogin = userLogin;
    }

    public UserLogin? UserLogin { get; set; }
};

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public LoginCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<LoginResponse> Handle(LoginCommand loginQuery, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();

        if (loginQuery.UserLogin == null)
            throw new ArgumentNullException(nameof(loginQuery.UserLogin)); // TODO exception mesajlarını değiştir

        if (string.IsNullOrWhiteSpace(loginQuery.UserLogin.UserName))
            throw new ArgumentNullException(nameof(loginQuery.UserLogin.UserName));

        if (string.IsNullOrWhiteSpace(loginQuery.UserLogin.Password))
            throw new ArgumentNullException(nameof(loginQuery.UserLogin.Password));

        var user = await _userManager.FindByNameAsync(loginQuery.UserLogin.UserName) ?? throw new UnauthorizedAccessException("Username or password is incorrect.");
        var result = await _signInManager.PasswordSignInAsync(user, loginQuery.UserLogin.Password, false, true);

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

        var userInfo = _mapper.Map<UserInfo>(user);

        return new LoginResponse { JWT = GenerateAccessToken(user), UserInfo = userInfo };
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
