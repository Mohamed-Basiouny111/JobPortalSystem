using JobPortalSystem.Context;
using JobPortalSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;



namespace JobPortalSystem.Repository
{
    [Authorize]
    public class JobFavoriteRepository : Controller
    {
        private readonly JobPortalContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobFavoriteRepository(JobPortalContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Show all favorite jobs for the logged-in user
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var favorites = await _context.FavoriteJobs
                .Include(f => f.Job)
                .Where(f => f.UserId == user.Id)
                .ToListAsync();

            return View(favorites);
        }

        // Add a job to favorites
        [HttpPost]
        public async Task<IActionResult> Add(int jobId)
        {
            var user = await _userManager.GetUserAsync(User);
            bool alreadyExists = await _context.FavoriteJobs
                .AnyAsync(f => f.UserId == user.Id && f.JobId == jobId);

            if (!alreadyExists)
            {
                var favorite = new JobFavorite { UserId = user.Id, JobId = jobId };
                _context.FavoriteJobs.Add(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Job", new { id = jobId });
        }

        // Remove a favorite job
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var favorite = await _context.FavoriteJobs.FindAsync(id);
            if (favorite != null)
            {
                _context.FavoriteJobs.Remove(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
