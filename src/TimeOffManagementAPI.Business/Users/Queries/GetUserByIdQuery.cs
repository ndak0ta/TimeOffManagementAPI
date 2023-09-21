using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Users.Queries;

public record GetUserByIdQuery : IRequest<UserInfo>
{
    public GetUserByIdQuery(string userId)
    {
        UserId = userId;
    }

    public string? UserId { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserInfo>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserInfo> Handle(GetUserByIdQuery getUserByTokenQuery, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByIdAsync(getUserByTokenQuery.UserId);

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        UserInfo userInfo = _mapper.Map<UserInfo>(user);
        userInfo.Roles = await _userManager.GetRolesAsync(user);

        return userInfo;
    }
}