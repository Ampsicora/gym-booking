using MVC_prenotazioni.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVC_prenotazioni.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<booking> ls = new List<booking>();
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync("https://localhost:44360/api/values");
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    ls = JsonConvert.DeserializeObject<List<booking>>(data.Result);
                }

            }
            return View(ls);
        }
        [HttpPost]
        public ActionResult Index(int id)
        {
            return View();
        }
        public ActionResult DeleteBooking()
        {
            List<booking> ls = new List<booking>();
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync(@"https://localhost:44360/api/values");
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    ls = JsonConvert.DeserializeObject<List<booking>>(data.Result);
                }
            }


            return View(ls);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}