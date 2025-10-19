using JobPortalSystem.Models;

namespace JobPortalSystem.Repository
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        Task<IEnumerable<Job>> GetJobsWithCategoriesAsync();
        Task<Job> GetJobWithCategoryAsync(int jobId);


        Task<IEnumerable<Job>> GetJobsByUserAsync(string userId);

        Task<IEnumerable<Job>> GetJobsByNameOrLocAsync(string searchWord);

        Task<IEnumerable<Job>> SearchJobsByCategoryAsync(int categoryId);


    }
}
