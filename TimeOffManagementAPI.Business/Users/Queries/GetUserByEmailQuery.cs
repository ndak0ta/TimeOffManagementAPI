using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Users.Queries;

public record GetUserByEmailQuery : IRequest<UserInfo>
{
    public GetUserByEmailQuery(string email)
    {
        Email = email;
    }

    public string? Email { get; set; }
}

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserInfo>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public GetUserByEmailQueryHandler(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserInfo> Handle(GetUserByEmailQuery getUserByEmailQuery, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(getUserByEmailQuery.Email);

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var userInfo = _mapper.Map<UserInfo>(user);
        userInfo.Roles = await _userManager.GetRolesAsync(user);

        return userInfo;
    }
}