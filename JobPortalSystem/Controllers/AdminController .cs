using Castle.Components.DictionaryAdapter.Xml;
using JobPortalSystem.Context;
using JobPortalSystem.Models;
using JobPortalSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortalSystem.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JobPortalContext db;

        public AdminController(UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager
            , JobPortalContext _db)
        {
            db= _db;    
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ✅ Show all users
        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
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


        // get all Companies
        public async Task<IActionResult> GetAllCompanies()
        {
            if (User.IsInRole("employer"))
            {
                var compnies = _userManager.Users.ToList();
                var comps = new List<ComUserInfo>();

                var obj = new ComUserInfo();

                foreach (var user in compnies)
                {   obj.Id= user.Id;  
                    obj.UserName = user.UserName;
                    obj.Email = user.Email;
                    obj.CompanyName = user.CompanyName;
                    comps.Add(obj);

                }



                return View("GetAllCompanies", comps);
            }



            return NoContent();
        }

           public async Task<IActionResult> Accept(ComUserInfo obFromRequst)
        {
            if (obFromRequst != null)
            {
                var objFromDB = await _userManager.FindByIdAsync(obFromRequst.Id);

                if (objFromDB != null) { 
                   objFromDB.CompanyIsAccepted = true;      
                
                }

            }

            return View();
        }
           public async Task< IActionResult> NotAccept(ComUserInfo obFromRequst)
        {
            if (obFromRequst != null)
            {
                var objFromDB = await _userManager.FindByIdAsync(obFromRequst.Id);

                if (objFromDB != null)
                {
                    objFromDB.CompanyIsAccepted = false;

                }

            }

            return View();
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
