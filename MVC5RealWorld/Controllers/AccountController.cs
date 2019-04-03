﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC5RealWorld.Models.DB;
using MVC5RealWorld.Models.EntityManager;
using MVC5RealWorld.Models.ViewModel;
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

        private void AuthenticationClaim(string loginName, string firstName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginName),
                new Claim("FullName", firstName),
                new Claim(ClaimTypes.Role, "Admin")
            };

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
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(UserLoginView ULV, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                string password = UM.GetUserPassword(ULV.LoginName);

                if (string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "The user login or password provided is incorrect.");
                }
                else
                {
                    if (ULV.Password.Equals(password))
                    {
                        AuthenticationClaim(ULV.LoginName, "Weslley");
                        return RedirectToAction("Welcome", "Home");
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
    }
}