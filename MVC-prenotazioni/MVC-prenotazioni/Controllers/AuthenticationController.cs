using MVC_prenotazioni.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync(@"https://localhost:44360/api/user/" + mail + "/" + pwd);
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    FormsAuthentication.SetAuthCookie(mail, false);
                    return RedirectToAction("Index", "Home");
                }
            }
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
               .Any(y => y == null ) || u.birthday > DateTime.Now)
            {
                ViewBag.Msg = "Registration Failed, you sent invalid data.";
                return View();
            }
            using (var conn = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(u), Encoding.UTF8, "application/json");
                var req = conn.PostAsync(@"https://localhost:44360/api/user/register", content);
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
            return RedirectToAction("Login");
        }
    }
}