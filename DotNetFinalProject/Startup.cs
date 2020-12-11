using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetFinalProject.Data;
using DotNetFinalProject.Models;
using DotNetFinalProject.Services;
using DotNetFinalProject.Data;
using DotNetFinalProject.Models;
using DotNetFinalProject.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotNetFinalProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CourseProjectContext>(options =>
            {
                options.UseSqlite("Filename=CourseProject.db");
            });
            
            
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<CourseProjectContext>();
            
            
            services.AddMvc(option => option.EnableEndpointRouting = false); 
            
            services.AddScoped<ProjectService>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<SpecialtyService>();
            services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
           
            
            // identity
            /*services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CourseProjectContext>();*/
            
            services.AddControllersWithViews();
            services.AddRazorPages();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            
            
            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Main}/{action=Index}/{id?}");
            });
            
            //CreateUserRoles(serviceProvider).Wait();
            

            app.UseStaticFiles();
            
            app.UseHttpsRedirection();

            app.UseRouting();

        }
    }
}