using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using JobPortalSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortalSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IJobRepository jobRepo;
        private readonly IGenericRepository<JobCategory> categoryRepo;

        public HomeController(ILogger<HomeController> logger, IJobRepository jobRepo , IGenericRepository<JobCategory> categoryRepo)
        {
            _logger = logger;
            this.jobRepo=jobRepo;
            this.categoryRepo=categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            //var jobsCategories = await categoryRepo.GetAllAsync();
            //var jobs = await jobRepo.GetJobsWithCategoriesAsync();
            JobsAndCategoriesVM jobsAndCategoriesVM = new JobsAndCategoriesVM()
            {
                jobs = await jobRepo.GetJobsWithCategoriesAsync(),
                Categories = await categoryRepo.GetAllAsync()
            };
            return View(jobsAndCategoriesVM);
        }

        public async Task<IActionResult> SearchForJobAsync(string searchWord)
        {
            var jobs = string.IsNullOrWhiteSpace(searchWord)
                       ? await jobRepo.GetJobsWithCategoriesAsync()
                       : await jobRepo.GetJobsByNameOrLocAsync(searchWord);
            JobsAndCategoriesVM jobsAndCategoriesVM = new JobsAndCategoriesVM()
            {
                jobs = jobs,
                Categories = await categoryRepo.GetAllAsync()
            };
            return PartialView("_HomeJobsPV", jobsAndCategoriesVM);
        }
     
        public async Task<IActionResult> SearchForJobByCategory(int categoryId)
        {
            IEnumerable<Job> jobs;
            if (categoryId == -1)
            {
                jobs = await jobRepo.GetJobsWithCategoriesAsync();
            }
            else
            {

                 jobs = await jobRepo.SearchJobsByCategoryAsync(categoryId);
               
            }
            JobsAndCategoriesVM jobsAndCategoriesVM = new JobsAndCategoriesVM()
            {
                jobs = jobs,
                Categories = await categoryRepo.GetAllAsync()

            };

            return PartialView("_HomeJobsPV", jobsAndCategoriesVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
