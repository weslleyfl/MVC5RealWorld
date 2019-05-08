using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using MVC5RealWorld.Models.DB;
using MVC5RealWorld.Models.Interface;
using MVC5RealWorld.Security;
using MVC5RealWorld.Util;
using MVC5RealWorld.Util.ActionAtribute;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            //services.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.ClearProviders();
            //    loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
            //    loggingBuilder.AddConsole();
            //    loggingBuilder.AddDebug();
            //});


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

            //services.AddMvc(options =>
            //{
            //    // options.Filters.Add(new ErrorHandlingFilter());
            //     //options.Filters.Add(typeof(ErrorHandlingFilter));
            //     options.Filters.Add(typeof(ApiExceptionFilter));

            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            // Database connection
            // services.AddScoped(typeof(DemoDBContext));
            services.AddDbContext<DemoDBContext>(options =>
                  options.UseSqlServer(connectionString: Configuration.GetConnectionString("DemoDatabaseConnection")));


            // IOptions - configure strongly typed settings objects
            // demonstrate the power of the IOptions<> pattern
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure 
            var appSettings = appSettingsSection.Get<AppSettings>();


            // Registers required services for health checks
            services.AddHealthChecksUI();

            services.AddHealthChecks()
            // Add a health check for a SQL database
            .AddCheck("MyDatabase", new SqlConnectionHealthCheck(Configuration["ConnectionStrings:DemoDatabaseConnection"]));

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

            // other code remove for clarity 

            _logger.LogInformation("In Development environment");

            app.UseStatusCodePages();

            
            //app.Use(async (ctx, next) =>
            //{
            //    await next();

            //    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
            //    {
            //        //Re-execute the request so the user gets the error page
            //        string originalPath = ctx.Request.Path.Value;
            //        ctx.Items["originalPath"] = originalPath;
            //        ctx.Request.Path = "/error/404";
            //        await next();
            //    }


            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                //app.UseExceptionHandler(errorApp =>
                //{
                //    errorApp.Run(async context =>
                //    {
                //        context.Response.StatusCode = 500;
                //        context.Response.ContentType = "text/html";

                //        await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                //        await context.Response.WriteAsync("ERROR!<br><br>\r\n");

                //        var exceptionHandlerPathFeature =
                //            context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

                //        // Use exceptionHandlerPathFeature to process the exception (for example, 
                //        // logging), but do NOT expose sensitive error information directly to 
                //        // the client.

                //        if (exceptionHandlerPathFeature?.Error is System.IO.FileNotFoundException)
                //        {
                //            await context.Response.WriteAsync("File error thrown!<br><br>\r\n");
                //        }

                //        await context.Response.WriteAsync("<a href=\"/\">Home</a><br>\r\n");
                //        await context.Response.WriteAsync("</body></html>\r\n");
                //        await context.Response.WriteAsync(new string(' ', 512)); // IE padding
                //    });
                //});
            }

            

            // use MVC middleware to configure error handling
            app.ConfigureExceptionHandler(_logger);

            // Definindo a cultura padrão: pt-BR
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            _logger.LogInformation(CultureInfo.CurrentCulture.DisplayName);


            // app.UseHttpsRedirection();

            //app.UseHealthChecks("/hc");


            // HealthCheck middleware
            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");
            // app.UseHealthChecksUI(setup => { setup.ApiPath = "/hc"; setup.UIPath = "/hc-ui"; });


            app.UseStaticFiles();
            app.UseCookiePolicy();

            // middleware do Identity para usar os seus recursos
            // que adiciona uma autenticação baseada em cookie ao pipeline de solicitação.
            app.UseAuthentication();

            app.UseSession();

            //app.UseMvcWithDefaultRoute();
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

                //routes.MapRoute(
                //         name: "login",
                //         template: "{controller}/{action}/{returnUrl}",
                //         defaults: new { controller = "Account", action = "LogIn", returnUrl = System.Web.Mvc.UrlParameter.Optional }
                //     );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id:int?}");

                routes.MapRoute(
                    name: "api",
                    //template: "api/{controller=Home}/{action=welcome}/{id:int?}");
                    template: "api/{controller}/{action}/{id?}",
                    defaults: new { controller = "home", action = "welcome" },
                    constraints: new { Id = @"\d+" });

            });

        }

        private static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));

            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }
    }
}
