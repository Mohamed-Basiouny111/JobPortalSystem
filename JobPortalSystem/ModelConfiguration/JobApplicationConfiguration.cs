using JobPortalSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobPortalSystem.ModelConfiguration
{
    public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
    {
        public void Configure(EntityTypeBuilder<JobApplication> builder)
        {
            builder.ToTable("JobApplication");
           
            //M:M relation 
            builder.HasOne(jf => jf.Job)
                    .WithMany(j => j.JobApplications)
                    .HasForeignKey(jf => jf.JobId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(jf => jf.User)
                .WithMany(u => u.JobApplications)
                .HasForeignKey(jf => jf.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(j => j.AppliedDate)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();
        }
    }
}
