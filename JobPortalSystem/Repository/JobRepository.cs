using JobPortalSystem.Context;
using JobPortalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Repository
{
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {

        public JobRepository(JobPortalContext context) : base(context)
        {

        }

        //Additional methods specific to Job entity can be added here
        public async Task<IEnumerable<Job>> GetJobsWithCategoriesAsync()
        {
            return await context.Jobs
                .Include(j => j.Category)
                .Include(j=>j.PostedUser)
                .ToListAsync();
        }

        public async Task<Job> GetJobWithCategoryAsync(int jobId)
        {
            return await context.Jobs
                .Include(j => j.Category)
                .FirstOrDefaultAsync(j => j.Id == jobId);
        }

        public async Task<IEnumerable<Job>> GetJobsByUserAsync(string userId)
        {
            return await context.Jobs
                                .Where(j => j.PostedByUserId == userId)
                                .Include(j => j.Category)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetSearchedJobAsync(string Searchedvalue)
        {
           

            Searchedvalue = Searchedvalue.Trim().ToLower();

            return await context.Jobs
                 .Include(j => j.Category)
                 .Include(j => j.PostedUser)
                 .Where(j => j.Title.ToLower().Contains(Searchedvalue) || j.Location.ToLower().Contains(Searchedvalue))
                 .ToListAsync();

        }
    }
}
