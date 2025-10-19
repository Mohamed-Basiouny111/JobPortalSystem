using JobPortalSystem.Models;
using JobPortalSystem.Repository;
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

        public HomeController(ILogger<HomeController> logger, IJobRepository jobRepo)
        {
            _logger = logger;
            this.jobRepo=jobRepo;
        }

        public async Task<IActionResult> Index()
        {
            var jobs = await jobRepo.GetJobsWithCategoriesAsync();
            return View(jobs);
        }

        public async Task<IActionResult> SearchForJobAsync(string searchWord)
        {
            var jobs = string.IsNullOrWhiteSpace(searchWord)
                       ? await jobRepo.GetJobsWithCategoriesAsync()
                       : await jobRepo.GetSearchedJobAsync(searchWord);

            return PartialView("_HomeJobsPV", jobs);
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
