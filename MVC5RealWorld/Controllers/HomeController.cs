using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Security.Application;
using MVC5RealWorld.Models;
using MVC5RealWorld.Models.EntityManager.Extensions;
using MVC5RealWorld.Models.Interface;
using MVC5RealWorld.Models.ViewModel;
using MVC5RealWorld.Util;
using MVC5RealWorld.Util.ActionAtribute;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace MVC5RealWorld.Controllers
{
   
    public class HomeController : Controller
    {
        private IPathProvider _pathProvider;
        private readonly IDateTime _dateTime;

        private readonly AppSettings _appSettings;

        private readonly ILogger<HomeController> _logger;

        public HomeController(  IPathProvider pathProvider, 
                                IDateTime dateTime, 
                                IOptions<AppSettings> appSettings, 
                                ILogger<HomeController> logger
                            )
        {
            _pathProvider = pathProvider;
            _dateTime = dateTime;
            _logger = logger;

            // demonstrate the power of the IOptions<> pattern
            _appSettings = appSettings.Value;
        }

        [AddHeader(name: "Autor", value: "WEslley Fernando Lopes")]
        //[ErrorHandlingFilter()]
        //[ApiExceptionFilter()]
        public IActionResult Index()
        {
            
            var serverTime = _dateTime.Now;

            var Message = $"About page visited at {DateTime.UtcNow.ToLongTimeString()}";
            _logger.LogDebug("Message displayed: {Message}", Message);
            _logger.LogInformation("Mensagem de informaçao para teste");
            _logger.LogInformation("Calling home controller action");

            //var exceptionHandlerPathFeature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

            //if (exceptionHandlerPathFeature?.Error is System.IO.FileNotFoundException)
            //{
            //    serverTime = _dateTime.Now;
            //}

            // throw new Exception("teste");

            // demonstrate the power of the IOptions<> pattern
            var teste = _appSettings.StringSetting;

            if (serverTime.Hour < 12)
            {
                ViewData["Message"] = "It's morning here - Good Morning!";
            }
            else if (serverTime.Hour < 17)
            {
                ViewData["Message"] = "It's afternoon here - Good Afternoon!";
            }
            else
            {
                ViewData["Message"] = "It's evening here - Good Evening!";
            }

            // System.Threading.Thread.CurrentPrincipal = HttpContext.User;
            //var thre = System.Threading.Thread.CurrentPrincipal;
            //var contexto = HttpContext.Request;

            return View();
        }

        [HttpGet("searchcategory/{category}/{subcategory=all}/")]
        public string[] SearchByProducts([FromQuery] string category, [FromRoute] string subcategory)
        {
            return new[]
            {
                    $"Category: {category}, Subcategory: {subcategory}"
            };
        }

        //[Route("sobre")]
        //[Route("home/sobre")]
        [HttpGet("sobre")]
        [HttpGet("home/sobre")]
        public IActionResult About([FromServices] IDateTime dateTime)
        {
            ViewData["Message"] = $"Current server time: {dateTime.Now}";

            return View();
        }

        [HttpPost]
        public IActionResult Index(UserModel userModel)
        {
            return View(userModel);
        }

        //[Authorize(Policy = "AcessoUserPolicy", Roles = "Admin")]
        [Authorize(Policy = "AcessoUserPolicy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[Authorize()]
        //[Authorize(Policy = "AcessoUserPolicy", Roles = "Admin")]
        //[Authorize(Roles = "Member")]
        [Authorize(Policy = "AcessoUserPolicy")]
        public ActionResult Welcome()
        {
            return View();
        }

        [Authorize(Policy = "AcessoUserPolicy")]
        public ActionResult AdminOnly()
        {
            return View();
        }
    }
}
