using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Users.Queries;

public record GetUserByUsernameQuery : IRequest<UserInfo>
{
    public GetUserByUsernameQuery(string username)
    {
        Username = username;
    }

    public string? Username { get; set; }
}

public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, UserInfo>
{
    private readonly UserManager<User> _userManaer;
    private readonly IMapper _mapper;

    public GetUserByUsernameQueryHandler(UserManager<User> userManager, IMapper mapper)
    {
        _userManaer = userManager;
        _mapper = mapper;
    }

    public async Task<UserInfo> Handle(GetUserByUsernameQuery getUserByUsernameQuery, CancellationToken cancellationToken)
    {
        var user = await _userManaer.FindByNameAsync(getUserByUsernameQuery.Username);

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var userInfo = _mapper.Map<UserInfo>(user);
        userInfo.Roles = await _userManaer.GetRolesAsync(user);

        return userInfo;
    }
}