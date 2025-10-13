using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.ViewModels
{
    public class RoleVM
    {
        [Display(Name ="Role Name")]
        public string RoleName { get; set; }
    }
}
