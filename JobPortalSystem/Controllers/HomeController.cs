using System.Diagnostics;
using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IJobRepository jobRepo;

        public HomeController(ILogger<HomeController> logger , IJobRepository jobRepo)
        {
            _logger = logger;
            this.jobRepo=jobRepo;
        }

        public IActionResult Index()
        {
           
                var jobs = jobRepo.GetJobsWithCategoriesAsync().Result;
                return View(jobs);
            
            
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
