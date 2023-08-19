using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;
using System.Text;
using MediatR;
using TimeOffManagementAPI.Business.Commands.Email;

namespace TimeOffManagementAPI.Business.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;
    private readonly IMediator _mediator;

    public UserService(IMapper mapper, UserManager<User> userManager, IEmailService emailService, IMediator mediator)
    {
        _mapper = mapper;
        _userManager = userManager;
        _emailService = emailService;
        _mediator = mediator;
    }

    public async Task<IEnumerable<UserInfo>> GetAllAsync()
    {
        var users = await _userManager.Users.Where(u => u.IsActive).ToListAsync();

        var userInfos = new List<UserInfo>();

        foreach (var user in users)
        {
            var userToAdd = _mapper.Map<UserInfo>(user);
            userToAdd.Roles = await _userManager.GetRolesAsync(user);
            userInfos.Add(userToAdd);
        }

        return userInfos;
    }

    public async Task<IEnumerable<User>> GetAllPasiveAsync()
    {
        return await _userManager.Users.Where(u => !u.IsActive).ToListAsync();
    }

    public async Task<User> GetByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IEnumerable<User>> GetByRoleAsync(string role)
    {
        return await _userManager.GetUsersInRoleAsync(role);
    }

    public async Task<UserInfo> GetUserInfoAsync(string userId)
    {
        var userInfo = _mapper.Map<UserInfo>(await _userManager.FindByIdAsync(userId));

        userInfo.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(userId));

        return userInfo;
    }

    public async Task<IdentityResult> CreateAsync(UserRegistration userRegistration)
    {
        var user = _mapper.Map<User>(userRegistration);

        user.UserName = CeateUsername(userRegistration.FirstName, userRegistration.LastName);

        var password = CreatePassword(8);

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Employee");
            await _mediator.Send(new SendEmailCommand(user.Email, "Your account has been created", $"Your username is {user.UserName} and your password is {password}"));
        }

        return result;
    }

    public async Task<UserUpdate> UpdateAsync(UserUpdate userUpdate)
    {
        var existing = await _userManager.FindByIdAsync(userUpdate.Id);

        existing.UserName = userUpdate.UserName ?? existing.UserName;
        existing.FirstName = userUpdate.FirstName ?? existing.FirstName;
        existing.LastName = userUpdate.LastName ?? existing.LastName;
        existing.Address = userUpdate.Address ?? existing.Address;
        existing.Email = userUpdate.Email ?? existing.Email;
        existing.PhoneNumber = userUpdate.PhoneNumber ?? existing.PhoneNumber;
        existing.DateOfBirth = userUpdate.DateOfBirth ?? existing.DateOfBirth;
        existing.HireDate = userUpdate.HireDate ?? existing.HireDate;

        await _userManager.UpdateAsync(existing);

        return userUpdate;
    }

    public async Task<IdentityResult> DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        user.IsActive = false;

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> UpdateContactAsync(UserUpdateContact userUpdate)
    {
        var user = await _userManager.FindByIdAsync(userUpdate.Id);

        user.Email = userUpdate.Email;
        user.PhoneNumber = userUpdate.PhoneNumber;

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

        return await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<IdentityResult> ChangePasswordAsync(UserChangePassword userChangePassword)
    {
        var user = await _userManager.FindByIdAsync(userChangePassword.Id);

        if (!await _userManager.CheckPasswordAsync(user, userChangePassword.OldPassword))
            throw new UnauthorizedAccessException("Password is incorrect");

        return await _userManager.ChangePasswordAsync(user, userChangePassword.OldPassword, userChangePassword.NewPassword);
    }

    public async Task<IdentityResult> UpdateRemaningAnnualTimeOff(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        int timeOffLeft = user.AnnualTimeOffs;

        var userWithTimeOffs = await _userManager.Users.Include(u => u.TimeOffs).FirstOrDefaultAsync(u => u.Id == userId)
        ?? throw new NullReferenceException("User not found");

        if (userWithTimeOffs.TimeOffs == null)
            return IdentityResult.Success;

        foreach (var timeOff in userWithTimeOffs.TimeOffs)
        {
            if (timeOff.IsApproved && timeOff.StartDate.Year == DateTime.Now.Year)
            {
                timeOffLeft -= timeOff.TotalDays;
            }
        }

        user.RemainingAnnualTimeOffs = timeOffLeft;

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> SetAnnualTimeOffAsync(string userId, int annualTimeOff)
    {
        var user = await _userManager.FindByIdAsync(userId);

        user.AnnualTimeOffs = annualTimeOff;

        user.AutomaticAnnualTimeOffIncrement = false;

        return await _userManager.UpdateAsync(user);
    }

    public async Task UpdateAnnualTimeOffAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            if (!user.AutomaticAnnualTimeOffIncrement)
                continue;

            decimal workYear = (DateTime.Now - user.HireDate).Days / 365;

            user.AnnualTimeOffs = Convert.ToInt32(workYear > 15 ? 26 : workYear > 5 ? 20 : workYear > 1 ? 14 : 0);

            await _userManager.UpdateAsync(user);
        }
    }

    public async Task<Role> GetRoleAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var roles = await _userManager.GetRolesAsync(user);

        return new Role { Name = roles.FirstOrDefault() };
    }

    private string CeateUsername(string? firstName, string? lastName)
    {
        if (firstName == null || lastName == null)
            throw new NullReferenceException("First name or last name is null");

        var username = firstName.ToLower() + "." + lastName.ToLower();

        var users = _userManager.Users.Where(u => u.UserName.StartsWith(username)).ToList();

        if (users.Count == 0)
            return username;

        return username + users.Count;
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
