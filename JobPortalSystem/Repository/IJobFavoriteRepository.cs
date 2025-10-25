using JobPortalSystem.Context;
using JobPortalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Repository
{
    public interface IJobFavoriteRepository
    {
        Task<IEnumerable<JobFavorite>> GetUserFavoritesAsync(string userId);
        Task<JobFavorite> GetFavoriteAsync(int id);
        Task<JobFavorite> GetFavoriteByJobAndUserAsync(int jobId, string userId);
        Task AddAsync(JobFavorite favorite);
        Task RemoveAsync(JobFavorite favorite);

        //Task DeleteAsync(int id);
        Task<bool> IsJobFavoriteAsync(int jobId, string userId);
    }

    public class JobFavoriteRepository : IJobFavoriteRepository
    {
        private readonly JobPortalContext _context;

        public JobFavoriteRepository(JobPortalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobFavorite>> GetUserFavoritesAsync(string userId)
        {
            return await _context.FavoriteJobs
                .Include(f => f.Job)
                    .ThenInclude(j => j.Category)
                .Include(f => f.Job)
                    .ThenInclude(j => j.PostedUser)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.AddedDate)
                .ToListAsync();
        }

        public async Task<JobFavorite> GetFavoriteAsync(int id)
        {
            return await _context.FavoriteJobs
                .Include(f => f.Job)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<JobFavorite> GetFavoriteByJobAndUserAsync(int jobId, string userId)
        {
            return await _context.FavoriteJobs
                .FirstOrDefaultAsync(f => f.JobId == jobId && f.UserId == userId);
        }

        public async Task AddAsync(JobFavorite favorite)
        {
            await _context.FavoriteJobs.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(JobFavorite favorite)
        {
            _context.FavoriteJobs.Remove(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsJobFavoriteAsync(int jobId, string userId)
        {
            return await _context.FavoriteJobs
                .AnyAsync(f => f.JobId == jobId && f.UserId == userId);
        }

        //public async Task DeleteAsync(int id)
        //{
        //  var favJob =await GetFavoriteAsync(id);
        //   await RemoveAsync(favJob);
        //}
    }
}