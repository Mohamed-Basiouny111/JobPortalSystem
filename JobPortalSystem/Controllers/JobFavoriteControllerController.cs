using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobPortalSystem.Controllers
{
    [Authorize]
    public class JobFavoriteController : Controller
    {
        private readonly IJobFavoriteRepository _favoriteRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobFavoriteController(IJobFavoriteRepository favoriteRepository, UserManager<ApplicationUser> userManager)
        {
            _favoriteRepository = favoriteRepository;
            _userManager = userManager;
        }

        // GET: Show all favorite jobs for the logged-in user
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await _favoriteRepository.GetUserFavoritesAsync(userId);
            return View(favorites);
        }

        // POST: Add job to favorites
        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int jobId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                // Check if already favorited
                var existingFavorite = await _favoriteRepository.GetFavoriteByJobAndUserAsync(jobId, userId);
                if (existingFavorite != null)
                {
                    return Json(new { success = false, message = "Job is already in favorites" });
                }

                var favorite = new JobFavorite
                {
                    JobId = jobId,
                    UserId = userId,
                    AddedDate = DateTime.Now
                };

                await _favoriteRepository.AddAsync(favorite);

                return Json(new { success = true, message = "Job added to favorites" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        // POST: Remove job from favorites
        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(int jobId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "User not authenticated" });
                }

                var favorite = await _favoriteRepository.GetFavoriteByJobAndUserAsync(jobId, userId);
                if (favorite == null)
                {
                    return Json(new { success = false, message = "Job not found in favorites" });
                }

                await _favoriteRepository.RemoveAsync(favorite);

                return Json(new { success = true, message = "Job removed from favorites" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        // Check if job is favorite
        [HttpGet]
        public async Task<JsonResult> IsFavorite(int jobId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { isFavorite = false });
                }

                var isFavorite = await _favoriteRepository.IsJobFavoriteAsync(jobId, userId);
                return Json(new { isFavorite });
            }
            catch (Exception)
            {
                return Json(new { isFavorite = false });
            }
        }
    }
}