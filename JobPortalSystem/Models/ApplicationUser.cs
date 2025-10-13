using Microsoft.AspNetCore.Identity;

namespace JobPortalSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        // URL or path to the Photo
        public string? PhotoURL { get; set; }
        // URL or path to the CV file
        public string? CV { get; set; } 
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public bool CompanyIsAccepted { get; set; } = false;

        //Navigation Properties 1 : M with User can have multiple Jobs
        public virtual List<Job> Jobs { get; set; } = new List<Job>();

        //Navigation Properties M:M with ApplicationUser : Job
        public virtual List<JobFavorite> JobFavorites { get; set; } = new List<JobFavorite>();

        //Navigation Properties 1:M with JobApplication
        public virtual List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }
}
