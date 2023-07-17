using Microsoft.EntityFrameworkCore;
using TimeOffManagementAPI.Data.Access.Contexts;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model.Models;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Data.Access.Repositories;

public class TimeOffRepository : ITimeOffRepository
{
    private readonly TimeOffManagementDBContext _context;

    public TimeOffRepository(TimeOffManagementDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TimeOff>> GetAllAsync()
    {
        if (_context.TimeOffs != null)
            return await _context.TimeOffs.ToListAsync();
        else
            return Enumerable.Empty<TimeOff>();
    }

    public async Task<TimeOff> GetByIdAsync(int id)
    {
        if (_context.TimeOffs != null)
        {
            var result = await _context.TimeOffs.FirstOrDefaultAsync(t => t.Id == id);

            if (result == null)
                throw new NotFoundException($"No time off request found with id {id}.");

            return result;
        }
        else
            throw new Exception("Internal server error.");
    }

    public async Task<IEnumerable<TimeOff>> GetByUserIdAsync(string userId)
    {
        if (_context.TimeOffs != null)
        {
            var result = await _context.TimeOffs.Where(t => t.UserId == userId.ToString()).ToListAsync();

            if (result == null)
                throw new NotFoundException($"No time off request found with id {userId}.");

            return result;
        }
        else
            throw new Exception("Internal server error.");
    }

    public async Task<TimeOff> CreateAsync(TimeOff TimeOff)
    {
        if (_context.TimeOffs != null)
        {
            var result = await _context.TimeOffs.AddAsync(TimeOff);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
        else
            throw new Exception("Internal server error.");
    }

    public async Task<TimeOff> UpdateAsync(TimeOff TimeOff)
    {
        if (_context.TimeOffs != null)
        {
            var result = _context.TimeOffs.Update(TimeOff);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
        else
            throw new Exception("Internal server error.");
    }

    public async Task DeleteAsync(int id)
    {
        if (_context.TimeOffs != null)
        {
            var result = await _context.TimeOffs.FirstOrDefaultAsync(t => t.Id == id);

            if (result == null)
                throw new NotFoundException($"No time off request found with id {id}.");

            result.IsActive = false;

            _context.TimeOffs.Update(result);
            await _context.SaveChangesAsync();
        }
        else
            throw new Exception("Internal server error.");
    }
}
