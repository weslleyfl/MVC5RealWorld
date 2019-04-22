using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC5RealWorld.Models;
using MVC5RealWorld.Models.EntityManager.Extensions;
using MVC5RealWorld.Models.Interface;
using MVC5RealWorld.Models.ViewModel;
using MVC5RealWorld.Util;

namespace MVC5RealWorld.Controllers
{
    public class HomeController : Controller
    {
        private IPathProvider _pathProvider;
        private readonly IDateTime _dateTime;

        public HomeController(IPathProvider pathProvider, IDateTime dateTime)                
        {
            _pathProvider = pathProvider;
             _dateTime = dateTime;
        }

        public IActionResult Index()
        {
            var serverTime = _dateTime.Now;

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

            return View();
        }

        public IActionResult About([FromServices] IDateTime dateTime)
        {
            ViewData["Message"] = $"Current server time: {dateTime.Now}";

            return View("Index");
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
