using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC5RealWorld.Models.DB;
using MVC5RealWorld.Models.EntityManager;
using MVC5RealWorld.Util;
using MVC5RealWorld.Util.ActionAtribute;

namespace MVC5RealWorld.Controllers
{

    // [LogTimeException()]
    public class ArticleController : Controller
    {
        
        //private IPathProvider _pathProvider;
        
        //public ArticleController(IPathProvider pathProvider)
        //{
        //    _pathProvider = pathProvider;        
        //}

        // GET: Article
        

        public ActionResult Index()
        {
            // var caminho = _pathProvider.MapPath("/Data/Data.txt");        

            var b = ModelState.IsValid;
            return View();
        }

        // GET: Article/Details/5       
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Article/Create
        //[HttpGet(Name = "Criar")] // Roteamento de atributo com atributos Http[Verb]
        //[Route("Article/Create")] // Roteamento de atributo
        public ActionResult Create()
        {       
            return View();
        }

        // POST: Article/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[System.Web.Mvc.ValidateInput(true)]
        public ActionResult Create(Article collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Article/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Article/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Article/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Article/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        // [NonAction]               
        public JsonResult IsUserAvailable(string title)
        {
            string username = title;

            UserManager UM = new UserManager();

            if (!UM.IsLoginNameExist(username))
            {
                return Json(true);
            }

            string suggestedUID = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} is not available.", username);

            for (int i = 1; i < 100; i++)
            {
                string altCandidate = username + i.ToString();
                if (!UM.IsLoginNameExist(altCandidate))
                {
                    suggestedUID = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} is not available. Try {1}.", username, altCandidate);
                    break;
                }
            }
            return Json(suggestedUID);
        }
        
        public ActionResult List(int param1, int param2, int param3)
        {
            return Content($"Deu certo {param1} {param2} {param3}");
        }
        
        
    }

}