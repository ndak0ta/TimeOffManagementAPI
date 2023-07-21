using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeOffManagementAPI.Business.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;

namespace TimeOffManagementAPI.Business.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public UserService(IMapper mapper, UserManager<User> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userManager.Users.Where(u => u.isActive).ToListAsync();
    }

    public async Task<IEnumerable<User>> GetAllPasiveAsync()
    {
        return await _userManager.Users.Where(u => !u.isActive).ToListAsync();
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

    public async Task<IdentityResult> CreateAsync(UserRegistration userRegistration)
    {
        var user = _mapper.Map<User>(userRegistration);

        await _userManager.AddToRoleAsync(user, "Employee");

        return await _userManager.CreateAsync(user, userRegistration.Password);
    }

    public async Task<IdentityResult> UpdateAsync(UserUpdate userUpdate)
    {
        var user = _mapper.Map<User>(userUpdate);

        if (await _userManager.CheckPasswordAsync(user, userUpdate.Password))
            throw new UnauthorizedAccessException("Password is incorrect");

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        user.isActive = false;

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> HardUpdate(User user)
    {
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<IdentityResult> ChangePasswordAsync(UserChangePassword userChangePassword)
    {
        var user = await _userManager.FindByIdAsync(userChangePassword.Id);

        if (await _userManager.CheckPasswordAsync(user, userChangePassword.OldPassword))
            throw new UnauthorizedAccessException("Password is incorrect");

        return await _userManager.ChangePasswordAsync(user, userChangePassword.OldPassword, userChangePassword.NewPassword);
    }

    public async Task<IdentityResult> UpdateRemaningAnnualTimeOff(string userId) // TODO değer geri döndürmeden direkt dbye kaydet
    {
        var user = await _userManager.FindByIdAsync(userId);

        int timeOffLeft = user.AnnualTimeOffs;

        var userWithTimeOffs = await _userManager.Users.Include(u => u.TimeOffs).FirstOrDefaultAsync(u => u.Id == userId);

        if (userWithTimeOffs == null)
            throw new NullReferenceException("User not found");

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

            if (workYear > 15)
                user.AnnualTimeOffs = 26;
            else if (workYear > 5)
                user.AnnualTimeOffs = 20;
            else if (workYear > 1)
                user.AnnualTimeOffs = 14;

            await _userManager.UpdateAsync(user);
        }
    }
}
