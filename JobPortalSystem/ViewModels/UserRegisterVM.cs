using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.ViewModels
{
    public class UserRegisterVM
    {
        [Display(Name = "User Name")]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Password and confirm Password do not match.")]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
