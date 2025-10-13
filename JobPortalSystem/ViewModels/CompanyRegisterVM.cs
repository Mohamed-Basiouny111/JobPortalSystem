using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.ViewModels
{
    public class CompanyRegisterVM : UserRegisterVM
    {
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Company Description")]
        public string CompanyDescription { get; set; }
    }
}
