using JobPortalSystem.Context;
using JobPortalSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortalSystem.Controllers
{
    [Authorize]
    public class JobFavoriteController : Controller
    {
        private readonly JobPortalContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobFavoriteController(JobPortalContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var favoriteJobs = await _context.FavoriteJobs
                .Include(f => f.Job)
                    .ThenInclude(j => j.PostedUser)
                .Include(f => f.Job)
                    .ThenInclude(j => j.Category)
                .Where(f => f.UserId == user.Id)
                .ToListAsync();

            return View(favoriteJobs);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int jobId)
        {
            var user = await _userManager.GetUserAsync(User);

            var existing = await _context.FavoriteJobs
                .FirstOrDefaultAsync(f => f.UserId == user.Id && f.JobId == jobId);

            bool isFavorite;

            if (existing == null)
            {
                _context.FavoriteJobs.Add(new JobFavorite
                {
                    UserId = user.Id,
                    JobId = jobId
                });
                isFavorite = true;
            }
            else
            {
                _context.FavoriteJobs.Remove(existing);
                isFavorite = false;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, isFavorite });
        }
    }
}