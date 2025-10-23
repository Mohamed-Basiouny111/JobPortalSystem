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
                user.PhotoURL = "/Images/default.png";
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
                user.PhotoURL = "/Images/default.png";
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
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var CopmID = User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            var ComPFromDb = await userManager.FindByIdAsync(CopmID);
            if (User.IsInRole("employer"))
            {



                var compInfo = new ComUserInfo()
                {
                    ImgURl = ComPFromDb.PhotoURL,
                    UserName = ComPFromDb.UserName,
                    Email = ComPFromDb.Email,
                    CompanyDescription = ComPFromDb.CompanyDescription,
                    CompanyName = ComPFromDb.CompanyName



                };


                if (ComPFromDb.CompanyIsAccepted == true)
                {
                    compInfo.CompanyStatus = "Accebted";
                }
                else
                {
                    compInfo.CompanyStatus = "Not Accebted";
                }

                return View("CompanyInfo", compInfo);
            }
            var UserInfo = new ComUserInfo()
            {
                ImgURl = ComPFromDb.PhotoURL,
                UserName = ComPFromDb.UserName,
                CV = ComPFromDb.CV,
                Email = ComPFromDb.Email,

            };



            return View("UserInfo", UserInfo);
        }
        [HttpGet]

        public async Task<IActionResult> EditCompany()
        {
            var CopmID = User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            var ComPFromDb = await userManager.FindByIdAsync(CopmID);
            var compInfo = new ComUserInfo()
            {
                Id = ComPFromDb.Id,
                ImgURl = ComPFromDb.PhotoURL,
                UserName = ComPFromDb.UserName,
                Email = ComPFromDb.Email,
                CompanyName = ComPFromDb.CompanyName,
                CompanyDescription = ComPFromDb.CompanyDescription,



            };

            return View("EditCompany", compInfo);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEditCompany(ComUserInfo compFromRequst)
        {

            var ComPFromDb = await userManager.FindByIdAsync(compFromRequst.Id);


            if (ModelState.IsValid || ComPFromDb != null)
            {

                if (compFromRequst.ImageFile != null && compFromRequst.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(compFromRequst.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await compFromRequst.ImageFile.CopyToAsync(stream);
                    }

                    ComPFromDb.PhotoURL = "/images/" + fileName;
                }

                if (!string.Equals(ComPFromDb.Email, compFromRequst.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var existingUser = await userManager.FindByEmailAsync(compFromRequst.Email);
                    if (existingUser != null && existingUser.Id != ComPFromDb.Id)
                    {
                        ModelState.AddModelError("Email", "This email is already used by another user.");
                        return View("EditCompany", compFromRequst);
                    }
                }


                ComPFromDb.CompanyName = compFromRequst.CompanyName;
                ComPFromDb.UserName = compFromRequst.UserName;
                ComPFromDb.Email = compFromRequst.Email;
                ComPFromDb.NormalizedEmail = compFromRequst.Email.ToUpper();
                ComPFromDb.CompanyDescription = compFromRequst.CompanyDescription;


                if (!string.IsNullOrEmpty(compFromRequst.Password))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(ComPFromDb);
                    var result = await userManager.ResetPasswordAsync(ComPFromDb, token, compFromRequst.Password);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to update password");
                        return View("EditCompany", compFromRequst);
                    }
                }

                var updateResult = await userManager.UpdateAsync(ComPFromDb);

                if (updateResult.Succeeded)
                {
                    TempData["SuccessMessage"] = "Your profile has been updated successfully!";
                    return RedirectToAction("Profile", "Account");
                }

                ModelState.AddModelError("", "Something went wrong while updating your profile.");
            }


            return View("EditCompany", compFromRequst);
        }


        [HttpGet]
        public async Task<IActionResult> EditUserInfo()
        {
            var userId = User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            var userFromDb = await userManager.FindByIdAsync(userId);

            if (userFromDb == null)
                return NotFound();

            var userInfo = new ComUserInfo
            {
                Id = userFromDb.Id,
                ImgURl = userFromDb.PhotoURL,
                UserName = userFromDb.UserName,
                Email = userFromDb.Email,
                CVFile = userFromDb.CVFile,
            };

            return View(userInfo);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEditUser(ComUserInfo userFromRequest)
        {
            var userFromDb = await userManager.FindByIdAsync(userFromRequest.Id);

            if (userFromDb == null)
                return NotFound();

            if (ModelState.IsValid)
            {

                if (userFromRequest.ImageFile != null && userFromRequest.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userFromRequest.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await userFromRequest.ImageFile.CopyToAsync(stream);
                    }

                    userFromDb.PhotoURL = "/images/" + fileName;
                }


                if (userFromRequest.CVFile != null && userFromRequest.CVFile.Length > 0)
                {

                    var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
                    var fileExt = Path.GetExtension(userFromRequest.CVFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExt))
                    {
                        ModelState.AddModelError("CVFile", "Only PDF, DOC, and DOCX files are allowed.");
                        return View("EditUserInfo", userFromRequest);
                    }

                    if (userFromRequest.CVFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("CVFile", "File too large. Maximum allowed size is 5 MB.");
                        return View("EditUserInfo", userFromRequest);
                    }

                    var cvFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cv");
                    if (!Directory.Exists(cvFolder))
                        Directory.CreateDirectory(cvFolder);


                    var cvFileName = Guid.NewGuid().ToString() + fileExt;
                    var cvPath = Path.Combine(cvFolder, cvFileName);


                    using (var stream = new FileStream(cvPath, FileMode.Create))
                    {
                        await userFromRequest.CVFile.CopyToAsync(stream);
                    }


                    userFromDb.CV = cvFileName;
                }


                if (!string.Equals(userFromDb.Email, userFromRequest.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var existingUser = await userManager.FindByEmailAsync(userFromRequest.Email);
                    if (existingUser != null && existingUser.Id != userFromDb.Id)
                    {
                        ModelState.AddModelError("Email", "This email is already used by another user.");
                        return View("EditUserInfo", userFromRequest);
                    }
                }


                userFromDb.UserName = userFromRequest.UserName;
                userFromDb.Email = userFromRequest.Email;
                userFromDb.NormalizedEmail = userFromRequest.Email.ToUpper();

                if (!string.IsNullOrEmpty(userFromRequest.Password))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(userFromDb);
                    var result = await userManager.ResetPasswordAsync(userFromDb, token, userFromRequest.Password);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to update password.");
                        return View("EditUserInfo", userFromRequest);
                    }

                }


                var updateResult = await userManager.UpdateAsync(userFromDb);

                if (updateResult.Succeeded)
                {
                    TempData["SuccessMessage"] = "Your profile has been updated successfully!";
                    return RedirectToAction("Profile", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong while updating your profile.");
                }
            }

            return View("EditUserInfo", userFromRequest);
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


      
    }

}
