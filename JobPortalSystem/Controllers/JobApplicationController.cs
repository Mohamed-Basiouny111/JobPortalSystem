using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalSystem.Controllers
{
    [Authorize]
    public class JobApplicationController : Controller
    {
        private readonly IGenericRepository<JobApplication> _jobAppRepo;
        private readonly IGenericRepository<Job> _jobRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobApplicationController(
            IGenericRepository<JobApplication> jobAppRepo,
            IGenericRepository<Job> jobRepo,
            UserManager<ApplicationUser> userManager)
        {
            _jobAppRepo = jobAppRepo;
            _jobRepo = jobRepo;
            _userManager = userManager;
        }

        // ✅ عرض الطلبات الخاصة بالمستخدم (Job Seeker)
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> MyApplications()
        {
            var user = await _userManager.GetUserAsync(User);
            var apps = await _jobAppRepo.GetAllAsync();
            var myApps = apps
                .Where(a => a.UserId == user.Id)
                .OrderByDescending(a => a.AppliedDate)
                .ToList();

            return View(myApps);
        }

        // ✅ عرض صفحة التقديم على وظيفة
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> Apply(int jobId)
        {
            var job = await _jobRepo.GetByIdAsync(jobId);
            if (job == null)
                return NotFound();

            ViewBag.Job = job;
            return View(new JobApplication { JobId = jobId });
        }

        [HttpPost]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> Apply(JobApplication model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // ✅ التأكد أن الوظيفة موجودة
            var job = await _jobRepo.GetByIdAsync(model.JobId ?? 0);
            if (job == null) return NotFound("Job not found");

            // ✅ منع التقديم المكرر
            var exists = (await _jobAppRepo.GetAllAsync())
                         .Any(a => a.JobId == job.Id && a.UserId == user.Id);
            if (exists)
            {
                TempData["Error"] = "You already applied to this job.";
                return RedirectToAction("GetAll", "Job");
            }

            // ✅ إنشاء الطلب
            var newApp = new JobApplication
            {
                JobId = job.Id,
                UserId = user.Id,
                CoverLetter = model.CoverLetter,
                Status = "Pending",
                AppliedDate = DateTime.Now
            };

            await _jobAppRepo.AddAsync(newApp);
            await _jobAppRepo.SaveAsync();

            TempData["Success"] = "Application submitted!";
            return RedirectToAction("MyApplications");
        }


        // ✅ عرض المتقدمين لوظيفة (Employer)
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Applicants(int jobId)
        {
            var apps = await _jobAppRepo.GetAllAsync();
            var jobApps = apps
                .Where(a => a.JobId == jobId)
                .OrderByDescending(a => a.AppliedDate)
                .ToList();

            ViewBag.JobId = jobId;
            return View(jobApps);
        }

        // ✅ تحديث حالة المتقدم (Employer)
        [Authorize(Roles = "Employer")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var app = await _jobAppRepo.GetByIdAsync(id);
            if (app == null) return NotFound();

            app.Status = status;
            _jobAppRepo.Update(app);
            await _jobAppRepo.SaveAsync();

            TempData["Success"] = $"Application marked as {status}";
            return RedirectToAction("Applicants", new { jobId = app.JobId });
        }
    }
}
