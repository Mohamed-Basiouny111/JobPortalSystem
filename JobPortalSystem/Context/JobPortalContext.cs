using JobPortalSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Context
{
    public class JobPortalContext :IdentityDbContext<ApplicationUser>
    {
        public JobPortalContext()
        {
            
        }

        public JobPortalContext(DbContextOptions<JobPortalContext> options) : base(options)
        {

        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobFavorite> jobFavorites { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Job).Assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}
