using App.Core.Application;
using App.Core.Domain.Entities;
using App.Core.Domain.Interfaces;
using App.Infrastructure.DataAccess;
using App.Infrastructure.DataAccess.DataContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.Client.WebUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            System.Console.WriteLine("[1] Startup::Constructor()");
            Configuration = configuration;
        }

        // [1] This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            System.Console.WriteLine("[2] Startup::ConfigureServices()");

            // Application Settings
            AppSettings appSettings = new AppSettings();
            Configuration.Bind("FileUpload", appSettings.FileUpload);
            services.AddSingleton(appSettings);

            services.AddDbContext<AppDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<AppDataContext>();

            services.AddTransient<ICatalogueUnitOfWork, CatalogueUnitOfWork>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IFileService, FileService>();

            services.AddRazorPages();
        }

        // [2] This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            System.Console.WriteLine("[3] Startup::Configure()");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}