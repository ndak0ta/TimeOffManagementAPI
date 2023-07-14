using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Access.Contexts;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Data.Model.Dtos;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Data.Access.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TimeOffManagementDBContext _context;
    private readonly IMapper _mapper;

    public UserRepository(TimeOffManagementDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        if (_context.Users != null)
            return await _context.Users.ToListAsync();
        else
            return Enumerable.Empty<User>();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        if (_context.Users != null)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Id == id.ToString()); // TODO sonra bak

            if (result == null)
                throw new NotFoundException($"No user found with id {id}.");

            return result;
        }
        else
            throw new Exception("Internal server error.");
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        if (_context.Users != null)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (result == null)
                throw new NotFoundException($"No user found with username {username}.");

            return result;
        }
        else
            throw new Exception("Internal server error.");
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        if (_context.Users != null)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (result == null)
                throw new NotFoundException($"No user found with email {email}.");

            return result;
        }
        else
            throw new Exception("Internal server error.");
    }

    public async Task<User> CreateAsync(User user)
    {
        if (_context.Users != null)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (result != null)
                throw new DuplicateRecordException($"User with username {user.UserName} already exists.");

            result = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (result != null)
                throw new DuplicateRecordException($"User with email {user.Email} already exists.");

            var createdEntry = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return createdEntry.Entity;
        }
        else
            throw new Exception("Internal server error.");
    }

    public async Task<User> UpdateAsync(User user)
    {
        if (_context.Users != null)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (result == null)
                throw new NotFoundException($"No user found with id {user.Id}.");

            result = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (result != null && result.Id != user.Id)
                throw new DuplicateRecordException($"User with username {user.UserName} already exists.");

            result = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (result != null && result.Id != user.Id)
                throw new DuplicateRecordException($"User with email {user.Email} already exists.");

            var updatedEntry = _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return updatedEntry.Entity;
        }
        else
            throw new Exception("Internal server error.");
    }

    public async Task<User> DeleteAsync(User user)
    {
        if (_context.Users != null)
        {
            var result = _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
        else
            throw new Exception("Internal server error.");
    }
}
