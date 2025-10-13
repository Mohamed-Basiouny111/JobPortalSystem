using JobPortalSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> RoleManager;
        public RoleController(RoleManager<IdentityRole> _roleManager)
        {
            RoleManager = _roleManager;
        }


        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole();
                identityRole.Name = roleVM.RoleName;

               IdentityResult result =  await RoleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return View();
                }

            }

            return View(roleVM);
        }
    }
}
