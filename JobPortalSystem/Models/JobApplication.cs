using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalSystem.Models
{
    //M : M relationship between User : Job
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        [Required]
        public int? JobId { get; set; }
        public Job? Job { get; set; }

        //Added by SQL Autmoatic when creating a new job
        public DateTime AppliedDate { get; set; }
        public string? CoverLetter { get; set; }

        //Pending / Accepted / Rejected
        [Required]
        public string Status { get; set; } = "Pending";

    }

}
