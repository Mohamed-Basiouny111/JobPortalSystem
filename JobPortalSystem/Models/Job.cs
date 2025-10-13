using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalSystem.Models
{
    [Table("Job")]
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Requirements { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Experience { get; set; }

        public decimal Salary { get; set; }

        //Added by SQL Autmoatic when creating a new job
        public DateTime PostedDate { get; set; }

        //Added by user when creating a new job
        [Required]
        public DateTime ExpiryDate { get; set; }

        //Active / Closed , 1 : 0
        [Required]
        public bool Active { get; set; } = true;

        //Navigation Properties 1:M with JobCategory

        public int? CategoryId { get; set; }
        public JobCategory? Category { get; set; }

        //Navigation Properties 1:M with ApplicationUser
      
        public string? PostedByUserId { get; set; }
        public ApplicationUser? PostedUser { get; set; }

        //Navigation Properties M:M with ApplicationUser : Job
        public virtual List<JobFavorite> JobFavorites { get; set; } = new List<JobFavorite>();

        //Navigation Properties 1:M with JobApplication
        public virtual List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }

}
