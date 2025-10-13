using JobPortalSystem.Context;
using JobPortalSystem.Models;

namespace JobPortalSystem.Repository
{
    public class JobRepository : GenericRepository<Job>
    {
        public JobRepository(JobPortalContext context) : base(context)
        {

        }

        //Additional methods specific to Job entity can be added here
    }
}
