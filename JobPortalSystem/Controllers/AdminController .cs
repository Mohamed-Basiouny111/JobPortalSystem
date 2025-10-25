using JobPortalSystem.Context;
using JobPortalSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JobPortalContext context;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, JobPortalContext context)

        {
            _userManager = userManager;
            _roleManager = roleManager;
            this.context=context;
        }

        // ✅ Show all users
        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        public async Task<IActionResult> JobApplications()
        {
            var applications = await context.JobApplications
                .Include(a => a.User)  // To get user details
                .Include(a => a.Job)   // To get related job details
                .OrderByDescending(a => a.AppliedDate)
                .ToListAsync();

            return View(applications);
        }

        // ✅ Edit roles for specific user
        public async Task<IActionResult> EditRoles(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();

            var model = new EditUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                CurrentRoles = userRoles,
                AvailableRoles = allRoles.Select(r => r.Name).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRoles(EditUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            // Remove all current roles
            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove old roles");
                return View(model);
            }

            // Add selected roles
            if (model.SelectedRoles != null && model.SelectedRoles.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to assign new roles");
                    return View(model);
                }
            }

            return RedirectToAction("Users");
        }
    }

    // ✅ Helper ViewModel
    public class EditUserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public IList<string> CurrentRoles { get; set; } = new List<string>();
        public List<string> AvailableRoles { get; set; } = new();
        public List<string> SelectedRoles { get; set; } = new();
    }
}
