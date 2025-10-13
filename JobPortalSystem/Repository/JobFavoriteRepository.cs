using JobPortalSystem.Context;
using JobPortalSystem.Models;

namespace JobPortalSystem.Repository
{
    public class JobFavoriteRepository : GenericRepository<JobFavorite>
    {
        public JobFavoriteRepository(JobPortalContext context) : base(context)
        {

        }

        //Additional methods specific to JobFavorite entity can be added here
    }
}
