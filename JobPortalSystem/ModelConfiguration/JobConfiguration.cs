using JobPortalSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobPortalSystem.ModelConfiguration
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            //  Job - Category 1:M 
            builder.HasOne(j => j.Category)
                   .WithMany(c => c.Jobs)
                   .HasForeignKey(j => j.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull);

            //  Job - ApplicationUser 1:M 
            builder.HasOne(j => j.PostedUser)
                   .WithMany(u => u.Jobs)
                   .HasForeignKey(j => j.PostedByUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(j => j.PostedDate)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(j => j.Salary)
              .HasColumnType("decimal(18,2)")
              .IsRequired();

            // Indexes (for better search)
            builder.HasIndex(j => j.Title);
            builder.HasIndex(j => j.Location);
        }
    }
}
