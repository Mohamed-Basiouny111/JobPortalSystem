using JobPortalSystem.Models;

namespace JobPortalSystem.ViewModels
{
    public class JobsAndCategoriesVM
    {
        public IEnumerable <Job> jobs = new List<Job>();

        public IEnumerable <JobCategory> Categories = new List<JobCategory>();
    }
}
