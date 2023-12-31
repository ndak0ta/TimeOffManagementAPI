using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Users.Queries;

public record GetAllUsersQuery : IRequest<IEnumerable<UserInfo>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserInfo>>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserInfo>> Handle(GetAllUsersQuery getAllUsersQuery, CancellationToken cancellationToken)
    {
        List<User> users = await _userManager.Users.Where(u => u.IsActive).ToListAsync();

        List<UserInfo> userInfos = new();

        foreach (User? user in users)
        {
            UserInfo userToAdd = _mapper.Map<UserInfo>(user);
            userToAdd.Roles = await _userManager.GetRolesAsync(user);
            userInfos.Add(userToAdd);
        }

        return userInfos;
    }
}