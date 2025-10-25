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
        private readonly JobPortalContext _context;

        public JobApplicationController(
            IGenericRepository<JobApplication> jobAppRepo,
            IGenericRepository<Job> jobRepo,
            UserManager<ApplicationUser> userManager,
            JobPortalContext context)
        {
            _jobAppRepo = jobAppRepo;
            _jobRepo = jobRepo;
            _userManager = userManager;
            _context = context;
        }

        // ✅ عرض الطلبات الخاصة بالمستخدم
        [Authorize(Roles = "job Seeker")]
        public async Task<IActionResult> MyApplications()
        {
            var user = await _userManager.GetUserAsync(User);

            var myApps = await _context.JobApplications
                .Include(a => a.Job)
                .ThenInclude(j => j.PostedUser)
                .Where(a => a.UserId == user.Id)
                .OrderByDescending(a => a.AppliedDate)
                .ToListAsync();

            return View(myApps);
        }

        // ✅ [GET] عرض صفحة التقديم — مع منع الدخول لو مقدم قبل كده
        [Authorize(Roles = "job Seeker")]
        [HttpGet]
        public async Task<IActionResult> Apply(int jobId)
        {
            var user = await _userManager.GetUserAsync(User);

            // 🔒 منع الدخول لصفحة التقديم لو مقدم بالفعل
            bool alreadyApplied = await _context.JobApplications
                .AnyAsync(a => a.JobId == jobId && a.UserId == user.Id);

            if (alreadyApplied)
            {
                TempData["Error"] = "⚠ You have already applied for this job.";
                return RedirectToAction("MyApplications");
            }

            var job = await _jobRepo.GetByIdAsync(jobId);
            if (job == null)
                return NotFound("Job not found");

            ViewBag.Job = job;
            return View(new JobApplication { JobId = jobId });
        }

        // ✅ [POST] تنفيذ التقديم — مع منع التقديم مرة أخرى
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "job Seeker")]
        public async Task<IActionResult> Apply(JobApplication model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // 🔒 تأكيد رفع الـ CV قبل التقديم
            if (string.IsNullOrEmpty(user.CV))
            {
                TempData["Error"] = "⚠ You must upload your CV before applying.";
                return RedirectToAction("EditUserInfo", "Account");
            }
            if (string.IsNullOrWhiteSpace(model.CoverLetter))
                {
                 ModelState.AddModelError("CoverLetter", "Please write a cover letter before submitting.");
                return View(model);
                }

            // 🔒 منع التقديم المكرر من السيرفر
            bool alreadyApplied = await _context.JobApplications
                .AnyAsync(a => a.JobId == model.JobId && a.UserId == user.Id);

            if (alreadyApplied)
            {
                TempData["Error"] = "⚠ You already applied for this job.";
                return RedirectToAction("MyApplications");
            }

            // ✅ إنشاء طلب جديد
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

        // ✅ عرض المتقدمين (لـ Employer فقط)
        [Authorize(Roles = "employer")]
        public async Task<IActionResult> Applicants(int? jobId)
        {
            var employer = await _userManager.GetUserAsync(User);
            if (employer == null)
                return Unauthorized();

            var employerJobs = await _context.Jobs
                .Where(j => j.PostedByUserId == employer.Id)
                .ToListAsync();

            ViewBag.EmployerJobs = employerJobs;

            var query = _context.JobApplications
                .Include(a => a.User)
                .Include(a => a.Job)
                .Where(a => employerJobs.Select(j => j.Id).Contains(a.JobId ?? 0));

            if (jobId.HasValue)
                query = query.Where(a => a.JobId == jobId.Value);

            var jobApps = await query.OrderByDescending(a => a.AppliedDate).ToListAsync();

            ViewBag.JobTitle = jobId.HasValue
                ? jobApps.FirstOrDefault()?.Job?.Title ?? "Unknown Job"
                : "All Your Job Applicants";
            ViewBag.ApplicantsCount = jobApps.Count;

            return View(jobApps);
        }

        // ✅ تحديث الحالة (قبول - رفض)
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

