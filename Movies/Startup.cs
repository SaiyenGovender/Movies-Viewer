using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using System.IO;

namespace Movies
{
    public class Startup
    {
        private string _contentRootPath = "";
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            //_contentRootPath = Environment.CurrentDirectory;
            string appRoot = env.ContentRootPath;
            Environment.SetEnvironmentVariable("DataDirectory", Path.Combine(appRoot, "App_Data"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // string conn = Configuration.GetConnectionString("MoviesContext");         
            // if (conn.Contains("%CONTENTROOTPATH%"))
            // {
            //    conn = conn.Replace("%CONTENTROOTPATH%", _contentRootPath);
            //  }

            string path = Path.GetDirectoryName("appsettings.json");

            //       services.AddDbContext<MoviesContext>(options =>
            //             options.UseSqlServer(Configuration.GetConnectionString("MoviesContext").Replace("[DataDirectory]",path)));

            var conn = Environment.ExpandEnvironmentVariables(Configuration.GetConnectionString("MoviesContext"));
            Console.WriteLine(path);
            services.AddDbContext<MoviesContext>(options =>
            options.UseSqlServer(conn));
            //            options.UseSqlServer(Configuration.GetConnectionString("MoviesContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
