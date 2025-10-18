using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.ViewModels
{
    public class JobAndCategoriesVM
    {
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

        [Required]
        [DataType(DataType.Date)]
       
        public DateTime ExpiryDate { get; set; }
        //public bool Active { get; set; } = true;

        [Display(Name = "Category")]

        [Required(ErrorMessage = "Category is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Category is required")]
        public int? CategoryId { get; set; }

        //public string? PostedByUserId { get; set; }
        public IEnumerable<JobCategory> Categories { get; set; } = new List<JobCategory>();
    }
}
