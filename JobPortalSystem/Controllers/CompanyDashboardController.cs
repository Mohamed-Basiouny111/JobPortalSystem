using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using JobPortalSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalSystem.Controllers
{
    [Authorize(Roles = "Employer")]
    public class CompanyDashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<Job> _jobRepo;
        private readonly IGenericRepository<JobApplication> _appRepo;

        public CompanyDashboardController(
            UserManager<ApplicationUser> userManager,
            IGenericRepository<Job> jobRepo,
            IGenericRepository<JobApplication> appRepo)
        {
            _userManager = userManager;
            _jobRepo = jobRepo;
            _appRepo = appRepo;
        }

        // ✅ عرض لوحة التحكم الخاصة بالشركة
        public async Task<IActionResult> Index()
        {
            // 🔹 جلب بيانات المستخدم الحالي (الشركة)
            var company = await _userManager.GetUserAsync(User);
            if (company == null)
                return Unauthorized();

            // 🔹 جلب كل الوظائف التابعة للشركة
            var allJobs = (await _jobRepo.GetAllAsync())
                .Where(j => j.PostedByUserId == company.Id)
                .ToList();

            // 🔹 جلب كل الطلبات
            var allApplications = await _appRepo.GetAllAsync();

            // 🔹 حساب الإحصائيات
            var stats = new CompanyStatsViewModel
            {
                CompanyName = company.CompanyName ?? company.UserName,
                TotalJobs = allJobs.Count,
                ActiveJobs = allJobs.Count(j => j.Active),
                ClosedJobs = allJobs.Count(j => !j.Active),
                TotalApplicants = allApplications.Count(a => allJobs.Select(j => j.Id).Contains(a.JobId ?? 0)),
                AcceptedApplicants = allApplications.Count(a => a.Status == "Accepted" && allJobs.Select(j => j.Id).Contains(a.JobId ?? 0)),
                PendingApplicants = allApplications.Count(a => a.Status == "Pending" && allJobs.Select(j => j.Id).Contains(a.JobId ?? 0)),
                RejectedApplicants = allApplications.Count(a => a.Status == "Rejected" && allJobs.Select(j => j.Id).Contains(a.JobId ?? 0))
            };

            return View(stats);
        }
    }
}