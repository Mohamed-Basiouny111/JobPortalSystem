using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using JobPortalSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortalSystem.Controllers
{

    public class JobController : Controller
    {
        private readonly IGenericRepository<JobCategory> jobCatgRepo;
        public IJobRepository JobRepo { get; }

        public JobController(IJobRepository jobRepo, IGenericRepository<JobCategory> jobCatgRepo)
        {
            this.JobRepo=jobRepo;
            this.jobCatgRepo=jobCatgRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            string userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            IEnumerable<Job> jobs;

            if (userRole?.ToLower()=="admin")
            {
                jobs = await JobRepo.GetJobsWithCategoriesAsync();
            }
            else
            {
              jobs = await JobRepo.GetJobsByUserAsync(userId);

            }
            return View("ShowAllJobs", jobs);
        }

        [HttpGet]
        public async Task<IActionResult> AddJob()
        {
            JobAndCategoriesVM job_Categ_VM = new JobAndCategoriesVM();
            var categories = (await jobCatgRepo.GetAllAsync()).ToList();
            job_Categ_VM.Categories =categories;
            return View(job_Categ_VM);
        }

        [HttpPost]
        public async Task<IActionResult> AddJob(JobAndCategoriesVM job_Categ_VM)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                Job newJob = new Job()
                {
                    Title = job_Categ_VM.Title,
                    Description = job_Categ_VM.Description,
                    Location = job_Categ_VM.Location,
                    Requirements = job_Categ_VM.Requirements,
                    Experience = job_Categ_VM.Experience,
                    Salary = job_Categ_VM.Salary,
                    CategoryId = job_Categ_VM.CategoryId,
                    PostedByUserId = userId,
                    ExpiryDate = job_Categ_VM.ExpiryDate.Date,

                };
                await JobRepo.AddAsync(newJob);
                await JobRepo.SaveAsync();
                TempData["SuccessMessage"] = "Job added successfully!";     
                return RedirectToAction("GetAll");


            }
            var categories = (await jobCatgRepo.GetAllAsync()).ToList();
            job_Categ_VM.Categories =categories;
            return View(job_Categ_VM);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {


            var job = await JobRepo.GetByIdAsync(id);
            if (job == null)
                return NotFound();
            var categories = (await jobCatgRepo.GetAllAsync()).ToList();
            JobAndCategoriesVM job_categ_VM = new JobAndCategoriesVM()
            {
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
                targetjob.Location=job.Location;
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
