using JobPortalSystem.Context;
using JobPortalSystem.Models;

namespace JobPortalSystem.Repository
{
    public class JobApplicationRepository : GenericRepository<JobApplication>
    {
        public JobApplicationRepository(JobPortalContext context) : base(context)
        {

        }
        //Additional methods specific to JobApplication entity can be added here

    }

}
