using JobPortalSystem.Context;
using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using JobPortalSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortalSystem.Controllers
{
    public class JobController : Controller
    {
        private readonly IGenericRepository<JobCategory> jobCatgRepo;
        private readonly IJobRepository JobRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JobPortalContext _context;

        public JobController(
            IJobRepository jobRepo,
            IGenericRepository<JobCategory> jobCatgRepo,
            UserManager<ApplicationUser> userManager,
            JobPortalContext context)
        {
            JobRepo = jobRepo;
            this.jobCatgRepo = jobCatgRepo;
            _userManager = userManager;
            _context = context;
        }

        // في الـ JobController، طريقة GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _userManager.GetUserAsync(User);

            // ✅ المفضلة
            if (user == null)
            {
                ViewBag.FavoriteJobIds = new List<int>();
            }
            else
            {
                ViewBag.FavoriteJobIds = await _context.FavoriteJobs
                    .Where(f => f.UserId == user.Id)
                    .Select(f => f.JobId)
                    .ToListAsync();
            }

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            string userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            IEnumerable<Job> jobs = await JobRepo.GetJobsWithCategoriesAsync();

            // ✅ إصلاح مشكلة appliedJobIds
            var appliedJobIds = new List<int>();
            if (user != null)
            {
                appliedJobIds = await _context.JobApplications
                    .Where(a => a.UserId == user.Id && a.JobId.HasValue)
                    .Select(a => a.JobId.Value)
                    .ToListAsync();
            }

            var viewModel = new JobsAndCategoriesVM
            {
                Jobs = jobs,
                appliedJobIds = appliedJobIds
            };

            return View("ShowAllJobs", viewModel);
        }


        // ✅ تعديل وظيفة
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var job = await JobRepo.GetByIdAsync(id);
            if (job == null)
                return NotFound();

            var categories = (await jobCatgRepo.GetAllAsync()).ToList();
            var job_categ_VM = new JobAndCategoriesVM()
            {
                Id = job.Id,
                Title = job.Title,
                Location = job.Location,
                Description = job.Description,
                Salary = job.Salary,
                CategoryId = job.CategoryId,
                Experience = job.Experience,
                Requirements = job.Requirements,
                Categories = categories,
                ExpiryDate = job.ExpiryDate,
            };

            return View("EditJob", job_categ_VM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(JobAndCategoriesVM job)
        {
            var targetjob = await JobRepo.GetByIdAsync(job.Id);
            if (ModelState.IsValid)
            {
                targetjob.Title = job.Title;
                targetjob.Location = job.Location;
                targetjob.Description = job.Description;
                targetjob.Salary = job.Salary;
                targetjob.CategoryId = job.CategoryId;
                targetjob.Experience = job.Experience;
                targetjob.Requirements = job.Requirements;
                targetjob.ExpiryDate = job.ExpiryDate.Date;

                JobRepo.Update(targetjob);
                await JobRepo.SaveAsync();

                TempData["SuccessMessage"] = "Job updated successfully!";
                return RedirectToAction("GetAll");
            }

            return View(job);
        }

        // ✅ حذف وظيفة
        public async Task<IActionResult> Delete(int id)
        {
            var job = await JobRepo.GetByIdAsync(id);
            if (job == null)
                return NotFound();

            await JobRepo.DeleteAsync(id);
            await JobRepo.SaveAsync();

            TempData["SuccessMessage"] = "Job deleted successfully!";
            return RedirectToAction("GetAll");
        }
    }
}
