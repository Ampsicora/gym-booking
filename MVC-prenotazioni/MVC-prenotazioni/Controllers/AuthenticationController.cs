using MVC_prenotazioni.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_prenotazioni.Controllers
{
    public class AuthenticationController : Controller
    {

        public ActionResult Login()
        {
            if (HttpContext.User.Identity.Name != "")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Msg = TempData["msg"];
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Login([FromBody] string mail, string pwd)
        {
            if (HttpContext.User.Identity.Name != "")
            {
                return RedirectToAction("Index", "Home");
            }
            if (mail == "prova@prova.it" && pwd == "ciao")
            {
                FormsAuthentication.SetAuthCookie(mail, true);
                HttpCookie id = new HttpCookie("user", "1");
                HttpCookie subscribed = new HttpCookie("subscribe", "1");
                id.HttpOnly = true;
                subscribed.HttpOnly = true;
                Response.AppendCookie(id);
                Response.AppendCookie(subscribed);
                return RedirectToAction("Index", "Home");
            }
            //using (var conn = new HttpClient())
            //{
            //    var req = conn.GetAsync(@"https://localhost:44360/api/user/" + mail + "/" + pwd);
            //    req.Wait();
            //    var res = req.Result;
            //    if (res.IsSuccessStatusCode)
            //    {
            //        FormsAuthentication.SetAuthCookie(mail, true);
            //        var data = res.Content.ReadAsStringAsync();
            //        data.Wait();
            //        HttpCookie id = new HttpCookie("id", data.Result);
            //        id.HttpOnly = true;
            //        Response.AppendCookie(id);
            //        return RedirectToAction("Index", "Home");
            //    }
            //}
            ViewBag.Msg = "Login Failed.";
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Register(user u)
        {
            if (u.GetType().GetProperties().Select(p => p.GetValue(u))
               .Any(y => y == null ))
            {
                ViewBag.Msg = "Registration Failed, you had null data.";
                return View();
            }
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync(@"https://localhost:44360/api/user/register");
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    TempData["msg"] = "Registration Complete, you can log in to your account.";
                    return RedirectToAction("Login");
                }
            }
            ViewBag.Msg = "Registration Failed.";
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Authentication");
        }
    }
}