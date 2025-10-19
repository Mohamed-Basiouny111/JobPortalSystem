using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using JobPortalSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<Job> _jobRepo;
        private readonly IGenericRepository<JobApplication> _appRepo;

        public AdminDashboardController(
            UserManager<ApplicationUser> userManager,
            IGenericRepository<Job> jobRepo,
            IGenericRepository<JobApplication> appRepo)
        {
            _userManager = userManager;
            _jobRepo = jobRepo;
            _appRepo = appRepo;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var jobs = await _jobRepo.GetAllAsync();
            var apps = await _appRepo.GetAllAsync();

            var model = new AdminStatsViewModel
            {
                TotalUsers = users.Count,
                TotalEmployers = users.Count(u => !string.IsNullOrEmpty(u.CompanyName)),
                TotalJobSeekers = users.Count(u => string.IsNullOrEmpty(u.CompanyName)),
                TotalJobs = jobs.Count(),
                ActiveJobs = jobs.Count(j => j.Active),
                ClosedJobs = jobs.Count(j => !j.Active),
                TotalApplications = apps.Count(),
                PendingApplications = apps.Count(a => a.Status == "Pending"),
                AcceptedApplications = apps.Count(a => a.Status == "Accepted"),
                RejectedApplications = apps.Count(a => a.Status == "Rejected")
            };

            return View(model);
        }
    }
}