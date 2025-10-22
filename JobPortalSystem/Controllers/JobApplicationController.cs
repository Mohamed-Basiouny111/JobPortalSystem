using JobPortalSystem.Context;
using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Controllers
{
    [Authorize]
    public class JobApplicationController : Controller
    {
        private readonly IGenericRepository<JobApplication> _jobAppRepo;
        private readonly IGenericRepository<Job> _jobRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JobPortalContext _context; // ✅ أضفنا الـ DbContext

        public JobApplicationController(
            IGenericRepository<JobApplication> jobAppRepo,
            IGenericRepository<Job> jobRepo,
            UserManager<ApplicationUser> userManager,
            JobPortalContext context) // ✅ استقبلناه هنا
        {
            _jobAppRepo = jobAppRepo;
            _jobRepo = jobRepo;
            _userManager = userManager;
            _context = context; // ✅ خزناه هنا
        }

        // ✅ عرض الطلبات الخاصة بالمستخدم (Job Seeker)
        [Authorize(Roles = "job Seeker")]
        public async Task<IActionResult> MyApplications()
        {
            var user = await _userManager.GetUserAsync(User);

            // ✅ التحميل بالعلاقات (Job + PostedUser)
            var myApps = await _context.JobApplications
                .Include(a => a.Job)
                .ThenInclude(j => j.PostedUser)
                .Where(a => a.UserId == user.Id)
                .OrderByDescending(a => a.AppliedDate)
                .ToListAsync();

            return View(myApps);
        }

        // ✅ [GET] عرض صفحة التقديم
        [Authorize(Roles = "job Seeker")]
        [HttpGet]
        public async Task<IActionResult> Apply(int jobId)
        {
            var job = await _jobRepo.GetByIdAsync(jobId);
            if (job == null)
                return NotFound("Job not found");

            ViewBag.Job = job;
            return View(new JobApplication { JobId = jobId });
        }

        // ✅ [POST] تنفيذ التقديم
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "job Seeker")]
        public async Task<IActionResult> Apply(JobApplication model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var job = await _jobRepo.GetByIdAsync(model.JobId ?? 0);
            if (job == null)
                return NotFound("Job not found");

            ViewBag.Job = job;

            if (string.IsNullOrWhiteSpace(model.CoverLetter))
            {
                ModelState.AddModelError("CoverLetter", "Please write a cover letter before submitting.");
                return View(model);
            }

            var alreadyApplied = (await _jobAppRepo.GetAllAsync())
                .Any(a => a.JobId == model.JobId && a.UserId == user.Id);

            if (alreadyApplied)
            {
                TempData["Error"] = "⚠ You already applied for this job.";
                return RedirectToAction("MyApplications");
            }

            var newApp = new JobApplication
            {
                JobId = model.JobId,
                UserId = user.Id,
                CoverLetter = model.CoverLetter,
                AppliedDate = DateTime.Now,
                Status = "Pending"
            };

            await _jobAppRepo.AddAsync(newApp);
            await _jobAppRepo.SaveAsync();

            TempData["Success"] = "✅ Application submitted successfully!";
            return RedirectToAction("MyApplications");
        }

        // ✅ عرض المتقدمين لوظيفة (Employer)
        [Authorize(Roles = "employer")]
        public async Task<IActionResult> Applicants(int? jobId)
        {
            var employer = await _userManager.GetUserAsync(User);
            if (employer == null)
                return Unauthorized();

            // 🟢 جلب الوظائف الخاصة بالـ Employer الحالي
            var employerJobs = await _context.Jobs
                .Where(j => j.PostedByUserId == employer.Id)
                .ToListAsync();

            // ✅ تأكد إن الـ ViewBag مش فاضي حتى لو مفيش وظائف
            ViewBag.EmployerJobs = employerJobs ?? new List<Job>();

            // 🟢 فلترة الطلبات على وظائف الـ Employer فقط
            var jobAppsQuery = _context.JobApplications
                .Include(a => a.User)
                .Include(a => a.Job)
                .Where(a => employerJobs.Select(j => j.Id).Contains(a.JobId ?? 0));

            if (jobId.HasValue)
                jobAppsQuery = jobAppsQuery.Where(a => a.JobId == jobId.Value);

            var jobApps = await jobAppsQuery
                .OrderByDescending(a => a.AppliedDate)
                .ToListAsync();

            ViewBag.JobTitle = jobId.HasValue
                ? jobApps.FirstOrDefault()?.Job?.Title ?? "Unknown Job"
                : "All Your Job Applicants";

            ViewBag.ApplicantsCount = jobApps.Count;

            return View(jobApps);
        }





        // ✅ تحديث حالة المتقدم (Employer)
        [Authorize(Roles = "employer")]
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
