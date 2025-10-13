using JobPortalSystem.Context;
using JobPortalSystem.Models;

namespace JobPortalSystem.Repository
{
    public class JobCategoryRepository : GenericRepository<JobCategory>
    {
        public JobCategoryRepository(JobPortalContext context) : base(context)
        {

        }
        //Additional methods specific to JobCategory entity can be added here
    }
}