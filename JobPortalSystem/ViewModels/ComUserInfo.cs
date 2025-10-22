using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.ViewModels
{
    public class ComUserInfo
    {
        public string? Id { get; set; }
        public string? ImgURl { get; set; } = "/wwwroot/imsges/ALWASIT.png";
        [Display(Name = "User Name")]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        public string? CompanyStatus { get; set; }
        public string? CV { get; set; }
        public IFormFile? CVFile { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Password and confirm Password do not match.")]

        public string? ConfirmPassword { get; set; }


        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }

        [Display(Name = "Company Description")]
        public string? CompanyDescription { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
