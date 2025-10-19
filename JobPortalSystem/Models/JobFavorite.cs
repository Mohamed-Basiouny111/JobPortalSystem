using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalSystem.Models
{
    //M : M relationship between User : Job
    public class JobFavorite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public int JobId { get; set; }

        [ForeignKey("JobId")]
        public Job Job { get; set; }
        //Added by SQL Autmoatic when creating a new JobFavorite
        public DateTime AddedDate { get; set; }

      
    }
}