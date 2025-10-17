using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.CustomValidation
{
    public class UniqueCategoryNameAttribute: ValidationAttribute
    {
       
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var categoryRepo = (IGenericRepository<JobCategory>)
               validationContext.GetService(typeof(IGenericRepository<JobCategory>))!;
            string  CategNameFromReq = value.ToString()!.Trim().ToLower();
            
           var currentcategoryId = (validationContext.ObjectInstance as JobCategory).Id;
           

            var existingCategory = categoryRepo.GetAllAsync().Result
                .FirstOrDefault(c => c.Name.Trim().ToLower() == CategNameFromReq && c.Id != currentcategoryId);

            if (existingCategory != null) 
            { 
                return new ValidationResult(" there is existing category With This Name , Category name must be unique.");
            }

            return ValidationResult.Success;

        }
    }
}
