using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TimeOffManagementAPI.Data.Access.Contexts;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Data.Access.Abstractions
{
    public abstract class BaseRepository<TEntity> where TEntity : class
    {
        private readonly TimeOffManagementDBContext _context;

        public BaseRepository(TimeOffManagementDBContext context)
        {
            _context = context;
        }

        private DbSet<TEntity> DbSet => _context.Set<TEntity>();

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if (DbSet != null)
                return await DbSet.AsNoTracking().ToListAsync();
            else
                return Enumerable.Empty<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            if (DbSet != null)
            {
                TEntity? result = await DbSet.FindAsync(id);

                if (result == null)
                    throw new NotFoundException($"No entity found with id {id}.");

                return result;
            }
            else
                throw new Exception("Internal server error.");
        }

        public async Task<IEnumerable<TEntity>> GetByPropertyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (DbSet != null)
            {
                return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
            }
            else
                throw new Exception("Internal server error.");
        }

        public async Task<TEntity> GetByPropertyFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (DbSet != null)
            {
                return await DbSet.AsNoTracking().Where(predicate).FirstAsync();
            }
            else
                throw new Exception("Internal server error.");
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (DbSet != null)
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> result = await DbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                return result.Entity;
            }
            else
                throw new Exception("Internal server error.");
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (DbSet != null)
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> result = DbSet.Update(entity);
                await _context.SaveChangesAsync();

                return result.Entity;
            }
            else
                throw new Exception("Internal server error.");
        }

        public async Task DeleteAsync(int id)
        {
            if (DbSet != null)
            {
                TEntity? result = await DbSet.FindAsync(id);

                if (result == null)
                    throw new NotFoundException($"No entity found with id {id}.");

                _context.Entry(result).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("Internal server error.");
        }
    }
}
