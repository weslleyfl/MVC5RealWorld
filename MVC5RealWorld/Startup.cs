using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC5RealWorld.Models.DB;
using MVC5RealWorld.Models.Interface;
using MVC5RealWorld.Security;
using MVC5RealWorld.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MVC5RealWorld
{
    public class Startup
    {
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
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

                options.AccessDeniedPath = new PathString("/home/error"); // /Erros/AcessoNegado

                //options.Cookie = new CookieBuilder()
                //{
                //    Name = ".NomeCookie",
                //    Expiration = new System.TimeSpan(0, 120, 0),
                //    //Se tiver um domínio...
                //    //Domain = ".site.com.br",
                //};

            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("AcessoUserPolicy", policy =>
                    policy.Requirements.Add(new PermiteAcessoUsuarioRequirement("Admin", "Member")));

            });

            services.AddSingleton<IAuthorizationHandler, PermiteAcessoUsuarioRequirementHandler>();
            services.AddSingleton<IPathProvider, PathProvider>();

            services.AddSingleton<IDateTime, SystemDateTime>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Database connection
            // services.AddScoped(typeof(DemoDBContext));
            services.AddDbContext<DemoDBContext>(options =>
                  options.UseSqlServer(connectionString: Configuration.GetConnectionString("DemoDatabaseConnection")));


            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new List<CultureInfo>
            //            {
            //                new CultureInfo("en-US"),
            //                new CultureInfo("de-CH"),
            //                new CultureInfo("fr-CH"),
            //                new CultureInfo("it-CH"),
            //                new CultureInfo("pt-br")
            //            };

            //    // options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
            //    options.DefaultRequestCulture = new RequestCulture(culture: "pt-br", uiCulture: "pt-br");
            //    options.SupportedCultures = supportedCultures;
            //    options.SupportedUICultures = supportedCultures;
            //});



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            _logger.LogInformation("In Development environment");


            // Definindo a cultura padrão: pt-BR
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            _logger.LogInformation(CultureInfo.CurrentCulture.DisplayName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            /// app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {
                //routes.MapRoute("blog", "blog/{*article}",
                //    defaults: new { controller = "Blog", action = "Article" });

                //routes.MapRoute(
                //     name: "login",
                //     template: "{controller=Account}/{action=LogIn}/{returnUrl?}",
                //     defaults: new
                //     {
                //         controller = "Account",
                //         action = "LogIn",
                //         returnUrl = System.Web.Mvc.UrlParameter.Optional
                //     });

                routes.MapRoute(
                         name: "login",
                         template: "{controller}/{action}/{returnUrl}",
                         defaults: new { controller = "Account", action = "LogIn", returnUrl = System.Web.Mvc.UrlParameter.Optional }
                     );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });

        }
    }
}
