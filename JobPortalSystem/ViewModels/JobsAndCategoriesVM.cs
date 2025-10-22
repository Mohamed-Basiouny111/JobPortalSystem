using JobPortalSystem.Models;

namespace JobPortalSystem.ViewModels
{
    public class JobsAndCategoriesVM
    {
        public IEnumerable<Job> Jobs { get; set; } = new List<Job>();
        public IEnumerable<Job> jobs { get; internal set; }
        public IEnumerable<JobCategory> Categories { get; set; } = new List<JobCategory>();
        public List<int> appliedJobIds { get; set; } = new List<int>();
    }
}
