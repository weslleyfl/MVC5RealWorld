using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Security.Application;
using MVC5RealWorld.Models.DB;
using MVC5RealWorld.Models.EntityManager;
using MVC5RealWorld.Models.ViewModel;
using MVC5RealWorld.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC5RealWorld.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(UserSignUpView USV)
        {
            if (!ModelState.IsValid) return View();

            UserManager UM = new UserManager();

            if (UM.IsLoginNameExist(USV.LoginName))
            {
                ModelState.AddModelError("", "Login Name already taken.");
                return View();
            }

            UM.AddUserAccount(USV);

            AuthenticationClaim(USV.LoginName, USV.FirstName);

            return RedirectToAction("Welcome", "Home");

        }

        public IActionResult SignUpNome(string nome)
        {
            return View();
        }

        private void AuthenticationClaim(string loginName, string firstName)
        {
            // Criando uma identidade para o usuário
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginName),
                new Claim(ClaimTypes.NameIdentifier, firstName),
                new Claim("FullName", firstName),
                new Claim(ClaimTypes.Role, "Member")
            };

            // Criando uma Identidade e associando-a ao ambiente.
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(2),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                // IsPersistent = true
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // associando-a ao ambiente. - ClaimsPrincipal
            System.Threading.Thread.CurrentPrincipal = HttpContext.User;

        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult LogIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost()]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(UserLoginView ULV, string returnUrl)
        {
            
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                string password = UM.GetUserPassword(Encoder.HtmlEncode(ULV.LoginName));

                // if ((string.IsNullOrEmpty(password)) || (!ULV.Password.Equals(password)))
                if (string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "The user login or password provided is incorrect.");
                }
                else
                {
                    if (ULV.Password.Equals(password))
                    {

                        // Forms authentication
                        // FormsAuthentication.SetAuthCookie(uname, false);

                        AuthenticationClaim(ULV.LoginName, ULV.LoginName);
                        //return RedirectToAction("Welcome", "Home");

                        return Redirect(returnUrl ?? Url.Action("Welcome", "Home")); //"Home/Welcome");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The password provided is incorrect.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form 
            return View(ULV);

        }

        [Authorize]
        public ActionResult SignOut()
        {
            //FormsAuthentication.SignOut();
            //return RedirectToAction("Index", "Home");

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");

        }

        //public ActionResult Index()
        //{
        //    EmailLembreteSenha model = new EmailLembreteSenha();
        //    model.UsuarioNome = "Eduardo Coutinho";
        //    model.UsuarioSenha = "123";
        //    model.DataSolicitacao = DateTime.Now;



        //    //string viewCode = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Views/TemplateEmailLembreteSenha.cshtml"));

        //}
    }
}