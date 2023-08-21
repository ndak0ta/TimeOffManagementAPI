using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Business.Users.Queries;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Users.Commands;

public record UpdateUserCommand : IRequest<UserInfo>
{
    public UpdateUserCommand(UserUpdate userUpdate)
    {
        UserUpdate = userUpdate;
    }

    public UserUpdate? UserUpdate { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserInfo>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdateUserCommandHandler(UserManager<User> userManager, IMapper mapper, IMediator mediator)
    {
        _userManager = userManager;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<UserInfo> Handle(UpdateUserCommand updateUserCommand, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(updateUserCommand.UserUpdate?.Id);

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.FirstName = updateUserCommand.UserUpdate?.FirstName ?? user.FirstName;
        user.LastName = updateUserCommand.UserUpdate?.LastName ?? user.LastName;
        user.Email = updateUserCommand.UserUpdate?.Email ?? user.Email;
        user.PhoneNumber = updateUserCommand.UserUpdate?.PhoneNumber ?? user.PhoneNumber;
        user.Address = updateUserCommand.UserUpdate?.Address ?? user.Address;
        user.DateOfBirth = updateUserCommand.UserUpdate?.DateOfBirth ?? user.DateOfBirth;
        user.HireDate = updateUserCommand.UserUpdate?.HireDate ?? user.HireDate;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new Exception("Failed to update user");

        return _mapper.Map<UserInfo>(user);
    }
}