using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "User Name Or Email")]
        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }

        [DisplayName("Remember me?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }


}
