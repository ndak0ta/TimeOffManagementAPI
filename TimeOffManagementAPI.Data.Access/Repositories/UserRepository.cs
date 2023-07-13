using Microsoft.EntityFrameworkCore;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Access.Contexts;
using TimeOffManagementAPI.Data.Model;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Data.Access.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TimeOffManagementDBContext _context;

    public UserRepository(TimeOffManagementDBContext context)
    {
        _context = context;
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
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (result == null)
                throw new NotFoundException($"No user found with id {id}.");

            return result;
        }
        else
            throw new NotFoundException($"No user found with id {id}.");
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        if (_context.Users != null)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (result == null)
                throw new NotFoundException($"No user found with username {username}.");

            return result;
        }
        else
            throw new NotFoundException($"No user found with username {username}.");
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
            throw new NotFoundException($"No user found with email {email}.");
    }

    public async Task<User> CreateAsync(User user)
    {
        if (_context.Users != null)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

            if (result != null)
                throw new DuplicateRecordException($"User with username {user.Username} already exists.");

            result = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (result != null)
                throw new DuplicateRecordException($"User with email {user.Email} already exists.");

            var createdEntry = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return createdEntry.Entity;
        }
        else
            throw new DuplicateRecordException($"User with username {user.Username} already exists.");
    }

    public async Task<User> UpdateAsync(User user)
    {
        if (_context.Users != null)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (result == null)
                throw new NotFoundException($"No user found with id {user.Id}.");

            result = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

            if (result != null && result.Id != user.Id)
                throw new DuplicateRecordException($"User with username {user.Username} already exists.");

            result = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (result != null && result.Id != user.Id)
                throw new DuplicateRecordException($"User with email {user.Email} already exists.");

            var updatedEntry = _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return updatedEntry.Entity;
        }
        else
            throw new NotFoundException($"No user found with id {user.Id}.");
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
            throw new NotFoundException($"No user found with id {user.Id}.");
    }
}
