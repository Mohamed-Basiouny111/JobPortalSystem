using JobPortalSystem.Context;
using JobPortalSystem.Models;
using JobPortalSystem.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(op =>
            {
                op.Password.RequiredLength = 4;
                op.Password.RequireNonAlphanumeric = false;
                op.Password.RequireUppercase = false;
                op.Password.RequireLowercase = false;
                op.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<JobPortalContext>();

            //DBContext
            builder.Services.AddDbContext<JobPortalContext>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
            });

            builder.Services.AddScoped<IGenericRepository<Job> , JobRepository>();
            builder.Services.AddScoped<IGenericRepository<JobApplication> , JobApplicationRepository>();
            builder.Services.AddScoped<IGenericRepository<JobCategory> , JobCategoryRepository>();
            builder.Services.AddScoped<IGenericRepository<JobFavorite> , JobFavoriteRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
