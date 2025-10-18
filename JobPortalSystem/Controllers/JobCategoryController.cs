using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalSystem.Controllers
{
    public class JobCategoryController : Controller
    {
        private readonly IGenericRepository<JobCategory> jobCategRepo;

        public JobCategoryController(IGenericRepository<JobCategory> jobCategRepo)
        {
            this.jobCategRepo=jobCategRepo;
        }

        public async Task<IActionResult> GetAll()
        {
            var data = await jobCategRepo.GetAllAsync();
            return View("ShowAllCategories", data);
        }

        [HttpGet]
        public IActionResult Add()
        {
               return View("AddCategory");
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(JobCategory Newcateg)
        {
            if (ModelState.IsValid)
            {
                await jobCategRepo.AddAsync(Newcateg);
                await jobCategRepo.SaveAsync();
                return RedirectToAction("GetAll");
            }
            return View("AddCategory" ,Newcateg);
        }

        public async Task<IActionResult> update(int id)
        {
            var category = await jobCategRepo.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return View("EditCategory",category);
        }

        [HttpPost]
        public async Task<IActionResult> update(JobCategory _category)
        {
            if (ModelState.IsValid)
            {
                var category = await jobCategRepo.GetByIdAsync(_category.Id);
                if (category == null)
                    return NotFound();
                category.Name = _category.Name;
                jobCategRepo.Update(category);
                await jobCategRepo.SaveAsync();
                return RedirectToAction("GetAll");
            }
           

            return View("EditCategory", _category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await jobCategRepo.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            await jobCategRepo.DeleteAsync(id);
            await jobCategRepo.SaveAsync();
            return RedirectToAction("GetAll");
        }

    }
}
