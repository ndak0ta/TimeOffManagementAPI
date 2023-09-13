using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Business.Email.Commands;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Users.Commands;

public record CreateUserCommand : IRequest<UserInfo>
{
    public CreateUserCommand(UserRegistration userRegistration)
    {
        UserRegistration = userRegistration;
    }

    public UserRegistration? UserRegistration { get; set; }
};

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserInfo>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateUserCommandHandler(UserManager<User> userManager, IMapper mapper, IMediator mediator)
    {
        _userManager = userManager;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<UserInfo> Handle(CreateUserCommand createUserCommand, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(createUserCommand.UserRegistration);

        user.UserName = CeateUsername(createUserCommand.UserRegistration?.FirstName, createUserCommand.UserRegistration?.LastName);

        var password = CreatePassword(8);

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Employee");
            await _mediator.Send(new SendEmailCommand(user.Email, "Your account has been created", $"Your username is {user.UserName} and your password is {password}"));
        }
        else if (result.Errors.Any())
        {
            var message = result.Errors.Select(e => e.Description).ToList();
            throw new Exception(string.Join(", ", message));
        }

        var userCreated = await _userManager.FindByNameAsync(user.UserName);
        var userInfo = _mapper.Map<UserInfo>(userCreated);

        return userInfo;
    }

    private string CeateUsername(string? firstName, string? lastName)
    {
        if (firstName == null || lastName == null)
            throw new NullReferenceException("First name or last name is null");

        var username = firstName.ToLower().Replace(" ", ".") + "." + lastName.ToLower();

        username = username
        .Replace("ç", "c")
        .Replace("ğ", "g")
        .Replace("ı", "i")
        .Replace("İ", "i")
        .Replace("ö", "o")
        .Replace("ş", "s")
        .Replace("ü", "u");

        var users = _userManager.Users.Where(u => u.UserName.StartsWith(username)).ToList();

        if (users.Count == 0)
            return username;

        return username + users.Count + 1;
    }

    private string CreatePassword(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890.,?!@#$%^&*()_+=-";
        StringBuilder res = new();
        Random rnd = new();
        while (0 < length--)
        {
            res.Append(valid[rnd.Next(valid.Length)]);
        }
        return res.ToString();
    }
}
