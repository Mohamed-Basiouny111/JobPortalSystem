using JobPortalSystem.Models;
using JobPortalSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortalSystem.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }

        [HttpGet]
        public IActionResult UserRegister()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRegister(UserRegisterVM userRegisterVM)
        {
            
            var existingUser = await userManager.FindByEmailAsync(userRegisterVM.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email Already Exist");
                return View(userRegisterVM);
            }

            if (ModelState.IsValid)
            {    //Mapping 
                ApplicationUser user = new ApplicationUser();
                user.UserName = userRegisterVM.UserName;
                user.Email = userRegisterVM.Email;
                user.PhotoURL = "/Images/Profile/default.png";
                //Save user to database 
                IdentityResult result = await userManager.CreateAsync(user, userRegisterVM.Password);
                if (result.Succeeded)
                {
                    //assign Role
                    await userManager.AddToRoleAsync(user, "Job Seeker");

                    //Cookies
                  //  await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Login");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }



                return RedirectToAction("Index", "Home");
            }

            return View(userRegisterVM);
        }

        [HttpGet]
        public IActionResult CompanyRegister()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanyRegister(CompanyRegisterVM companyRegisterVM)
        {
            var existingUser = await userManager.FindByEmailAsync(companyRegisterVM.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email Already Exist");
                return View(companyRegisterVM);
            }

            if (ModelState.IsValid)
            {    //Mapping 
                ApplicationUser user = new ApplicationUser();
                user.UserName = companyRegisterVM.UserName;
                user.Email = companyRegisterVM.Email;
                user.CompanyName = companyRegisterVM.CompanyName;
                user.CompanyDescription = companyRegisterVM.CompanyDescription;
                user.PhotoURL = "/Images/Profile/default.png";
                user.CompanyIsAccepted = false;

                //Save user to database 
                IdentityResult result = await userManager.CreateAsync(user, companyRegisterVM.Password);
                if (result.Succeeded)
                {
                    //assign Role
                    await userManager.AddToRoleAsync(user, "employer");
                    //Cookies
                   // await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Login");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return RedirectToAction("Index", "Home");
            }

            return View(companyRegisterVM);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            returnUrl ??= Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                //check found 
                ApplicationUser user = await userManager.FindByNameAsync(loginVM.UserName);

                if (user == null)
                {
                    user = await userManager.FindByEmailAsync(loginVM.UserName);
                }

                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, loginVM.Password);
                    if (found)
                    { //Add Claim
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim("PhotoURL", user.PhotoURL));

                        //create Cookie 
                        await signInManager.SignInWithClaimsAsync(user, loginVM.RememberMe,claims);

                        if (Url.IsLocalUrl(returnUrl))
                            return LocalRedirect(returnUrl);
                        else
                            return RedirectToAction("Index", "Home");

                    }
                }

                ModelState.AddModelError("", "User Name , Email Or Password Invalid");

            }
            ViewData["ReturnUrl"] = returnUrl;
            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        //Soon
        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult test()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                while (User.IsInRole("admin"))
                {
                    // Admin specific logic
                }

                //  string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                string userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                string userName = User.Identity.Name;
                string photo = User.Claims.FirstOrDefault(c=>c.Type == "PhotoURL").Value;
            }

            return View();
        }


        public IActionResult Profile()
        {
            return View("Profile");
        }
    }

}
