using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MVC5RealWorld.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC5RealWorld
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddDbContext<DemoDBContext>(options =>
            //      options.UseSqlServer(Configuration.GetConnectionString("DemoDatabaseConnection"), providerOptions => providerOptions.CommandTimeout(60)));

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
          
            //var connection = @"data source=9932c;initial catalog=DemoDB;integrated security=True;MultipleActiveResultSets=True;App=DemoDB";
            //services.AddDbContext<DemoDBContext>(options => 
            //    options.UseSqlServer(connection));

            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = "CookieDemoLogin";
                options.ExpireTimeSpan = new TimeSpan(0, 5, 0);

                options.LoginPath = new PathString("/Account/Login"); // /Conta/Login
                options.LogoutPath = new PathString("/Account/SignUp"); // /Conta/Logout
                
                
                options.AccessDeniedPath = new PathString("/Erros/AcessoNegado"); // /Erros/AcessoNegado

                //options.Cookie = new CookieBuilder()
                //{
                //    Name = ".NomeCookie",
                //    Expiration = new System.TimeSpan(0, 120, 0),
                //    //Se tiver um domínio...
                //    //Domain = ".site.com.br",
                //};

            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Database connection
            // services.AddScoped(typeof(DemoDBContext));
            services.AddDbContext<DemoDBContext>(options =>
                  options.UseSqlServer(connectionString: Configuration.GetConnectionString("DemoDatabaseConnection")));



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();            
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
