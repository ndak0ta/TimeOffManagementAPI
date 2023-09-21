using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Data.Model.Models;

namespace TimeOffManagementAPI.Business.Users.Commands;

public record UpdateUserContactCommand : IRequest<UserInfo>
{
    public UpdateUserContactCommand(UserUpdateContact userUpdateContact)
    {
        UserUpdateContact = userUpdateContact;
    }

    public UserUpdateContact UserUpdateContact { get; set; }
}

public class UpdateUserContactCommandHandler : IRequestHandler<UpdateUserContactCommand, UserInfo>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UpdateUserContactCommandHandler(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserInfo> Handle(UpdateUserContactCommand updateUserContactCommand, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByIdAsync(updateUserContactCommand.UserUpdateContact.Id);

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.Email = updateUserContactCommand.UserUpdateContact.Email ?? user.Email;
        user.PhoneNumber = updateUserContactCommand.UserUpdateContact.PhoneNumber ?? user.PhoneNumber;

        IdentityResult result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new Exception("Failed to update user");

        return _mapper.Map<UserInfo>(user);
    }
}