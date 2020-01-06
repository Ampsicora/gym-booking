using MVC_prenotazioni.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVC_prenotazioni.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = TempData["msg"];
            List<room> ls = new List<room>();
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync("https://localhost:44360/api/values/getrooms");
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    ls = JsonConvert.DeserializeObject<List<room>>(data.Result);
                }
            }
            ViewBag.Rooms = ls;
            return View();
        }

        [HttpPost]
        public ActionResult PostIndex()
        {
            var test = HttpContext.Request.Form;
            using (var conn = new HttpClient())
            {
                HttpCookieCollection c = Response.Cookies;
                booking b = new booking()
                {
                    id_room = int.Parse(test.Get("room")),
                    email_user = c.Get("user").Value,
                    begin_time = TimeSpan.Parse(test.Get("start")),
                    end_time = TimeSpan.Parse(test.Get("endtime")),
                    equipment = test.Get("equipment") != null ? true : false,
                };
                TimeSpan interval = b.end_time - b.begin_time;
                b.price = (decimal)((c.Get("subscribe").Value == "false" ? (interval.TotalMinutes / 30) * 7 : 0) + (!b.equipment ? 3 : 0));
                StringContent content = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
                var req2 = conn.PostAsync(@"https://localhost:44360/api/values/newbooking", content);
                req2.Wait();
                TempData["msg"] = req2.Result.IsSuccessStatusCode ? "Successfully Booked. Total Price is: " + b.price
                                                                    : "I'm sorry, your booking wasn't confirmed.";
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteBooking()
        {
            List<booking> ls = new List<booking>();
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync(@"https://localhost:44360/api/values/bookingsperuser/" + Response.Cookies.Get("user").Value);
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

        public ActionResult Delete(int id)
        {
            using (var conn = new HttpClient())
            {
                var req = conn.DeleteAsync(@"https://localhost:44360/api/values/" + Response.Cookies.Get("user").Value + "/" + id);
                req.Wait();
                var res = req.Result;
                ViewBag.Message = res.IsSuccessStatusCode ? "Delete Successfull" : "Delete wasn't performed.";
            }
            return View();
        }

        public ActionResult UpdateBooking()
        {
            List<booking> ls = new List<booking>();
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync(@"https://localhost:44360/api/values/bookingsperuser/" + Response.Cookies.Get("user").Value);
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
        public ActionResult Update(booking b)
        {
            return UpdateBooking();

        }
        public ActionResult AllBookings()
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
    }
}