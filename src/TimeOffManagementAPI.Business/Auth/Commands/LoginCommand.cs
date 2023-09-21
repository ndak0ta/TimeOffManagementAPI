using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

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

        User user = await _userManager.FindByNameAsync(loginQuery.UserLogin.UserName) ?? throw new UnauthorizedAccessException("Username or password is incorrect.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Your account is not active. Please contact your manager.");

        SignInResult result = await _signInManager.PasswordSignInAsync(user, loginQuery.UserLogin.Password, false, true);

        if (result.Succeeded)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, null);
        }
        else if (result.IsLockedOut)
        {
            DateTimeOffset? lockoutEndDate = await _userManager.GetLockoutEndDateAsync(user);
            int timeLeft = lockoutEndDate.Value.Subtract(DateTimeOffset.UtcNow).Minutes + 1;
            throw new UnauthorizedAccessException($"Your account is locked out. Please try again {timeLeft} minutes later.");
        }
        else
        {
            await _userManager.AccessFailedAsync(user);
            throw new UnauthorizedAccessException("Username or password is incorrect.");
        }

        UserInfo userInfo = _mapper.Map<UserInfo>(user);

        return new LoginResponse { JWT = GenerateAccessToken(user), UserInfo = userInfo };
    }

    private string GenerateAccessToken(User user)
    {
        string? key = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        string? issuer = _configuration["Jwt:Issuer"];
        string? audience = _configuration["Jwt:Audience"];
        List<Claim> claims = new()
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.Integer64)
        };

        _userManager.GetRolesAsync(user).Result.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

        DateTime expires = DateTime.UtcNow.AddDays(1); // TODO sonra değiştir
        SigningCredentials signingCredentials = new(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
