using JobPortalSystem.Context;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly JobPortalContext context;
        protected readonly DbSet<T> table;

        public GenericRepository(JobPortalContext _context)
        {
            context = _context;
            table = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await table.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await table.AddAsync(entity);
        }

        public void Update(T entity)
        {
            table.Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                table.Remove(entity);
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }

}
