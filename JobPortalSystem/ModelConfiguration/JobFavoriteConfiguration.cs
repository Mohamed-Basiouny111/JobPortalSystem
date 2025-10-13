using JobPortalSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobPortalSystem.ModelConfiguration
{
    public class JobFavoriteConfiguration : IEntityTypeConfiguration<JobFavorite>
    {
        public void Configure(EntityTypeBuilder<JobFavorite> builder)
        {
            builder.ToTable("JobFavorite");

            //M:M relation 
            builder.HasOne(j => j.Job)
                    .WithMany(j => j.JobFavorites)
                    .HasForeignKey(j => j.JobId)
                    .OnDelete(DeleteBehavior.Restrict);
    
                builder.HasOne(j => j.User)
                    .WithMany(u => u.JobFavorites)
                    .HasForeignKey(j => j.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
    
                builder.Property(j => j.AddedDate)
                    .HasDefaultValueSql("GETDATE()")
                    .IsRequired();
        }
    }
}
