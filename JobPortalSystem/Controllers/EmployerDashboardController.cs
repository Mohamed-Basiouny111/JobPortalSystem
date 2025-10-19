using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalSystem.Controllers
{
    [Authorize(Roles = "Employer")]
    public class EmployerDashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<Job> _jobRepo;
        private readonly IGenericRepository<JobApplication> _appRepo;

        public EmployerDashboardController(
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
            var user = await _userManager.GetUserAsync(User);
            var jobs = (await _jobRepo.GetAllAsync()).Where(j => j.PostedByUserId == user.Id).ToList();
            var apps = await _appRepo.GetAllAsync();

            int totalApplicants = apps.Count(a => jobs.Select(j => j.Id).Contains(a.JobId ?? 0));

            var model = new EmployerStatsViewModel
            {
                TotalJobs = jobs.Count,
                TotalApplicants = totalApplicants,
                ActiveJobs = jobs.Count(j => j.Active),
                ClosedJobs = jobs.Count(j => !j.Active)
            };

            return View(model);
        }
    }

    public class EmployerStatsViewModel
    {
        public int TotalJobs { get; set; }
        public int TotalApplicants { get; set; }
        public int ActiveJobs { get; set; }
        public int ClosedJobs { get; set; }
    }
}