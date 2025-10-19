using JobPortalSystem.CustomValidation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalSystem.Models
{
    [Table("JobCategory")]
    public class JobCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]

        [UniqueCategoryName]
        public string Name { get; set; }

        //Navigation Properties
        public virtual List<Job> Jobs { get; set; } = new List<Job>();
    }
}
