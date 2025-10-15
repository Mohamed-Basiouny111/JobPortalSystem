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
        public int? JobId { get; set; }
        public virtual Job? Job { get; set; }

        [Required]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        //Added by SQL Autmoatic when creating a new JobFavorite
        public DateTime AddedDate { get; set; }

      
    }
}