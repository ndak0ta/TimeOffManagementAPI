using Microsoft.EntityFrameworkCore;
using TimeOffManagementAPI.Data.Access.Contexts;
using TimeOffManagementAPI.Data.Access.Interfaces;
using TimeOffManagementAPI.Data.Model;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Data.Access.Repositories;

public class TimeOffRepository : ITimeOffRepository
{
    private readonly TimeOffManagementDBContext _context;

    public TimeOffRepository(TimeOffManagementDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TimeOffRequest>> GetAllAsync()
    {
        if (_context.TimeOffRequests != null)
            return await _context.TimeOffRequests.ToListAsync();
        else
            return Enumerable.Empty<TimeOffRequest>();
    }

    public async Task<TimeOffRequest> GetByIdAsync(int id)
    {
        if (_context.TimeOffRequests != null)
        {
            var result = await _context.TimeOffRequests.FirstOrDefaultAsync(t => t.Id == id);

            if (result == null)
                throw new NotFoundException($"No time off request found with id {id}.");

            return result;
        }
        else
            throw new NotFoundException($"No time off request found with id {id}.");
    }

    public async Task<IEnumerable<TimeOffRequest>> GetByUserIdAsync(int userId)
    {
        if (_context.TimeOffRequests != null)
        {
            var result = await _context.TimeOffRequests.Where(t => t.userId == userId).ToListAsync();

            if (result == null)
                throw new NotFoundException($"No time off request found with id {userId}.");

            return result;
        }
        else
            throw new NotFoundException($"No time off request found with id {userId}.");

    }

    public async Task<TimeOffRequest> CreateAsync(TimeOffRequest timeOffRequest)
    {
        if (_context.TimeOffRequests != null)
        {
            var result = await _context.TimeOffRequests.AddAsync(timeOffRequest);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
        else
            throw new KeyNotFoundException($"No time off request found with id {timeOffRequest.Id}.");
    }

    public async Task<TimeOffRequest> UpdateAsync(TimeOffRequest timeOffRequest)
    {
        if (_context.TimeOffRequests != null)
        {
            var result = _context.TimeOffRequests.Update(timeOffRequest);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
        else
            throw new KeyNotFoundException($"No time off request found with id {timeOffRequest.Id}.");
    }

    public async Task<TimeOffRequest> DeleteAsync(TimeOffRequest timeOffRequest)
    {
        if (_context.TimeOffRequests != null)
        {
            var result = _context.TimeOffRequests.Remove(timeOffRequest);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
        else
            throw new KeyNotFoundException($"No time off request found with id {timeOffRequest.Id}.");
    }
}
