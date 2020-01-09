using MVC_prenotazioni.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            TimeSpan interval;
            if (ValidationData(test).Equals(new TimeSpan()))
            {
                return RedirectToAction("Index");
            } else
            {
                interval = ValidationData(test);
            }
            using (var conn = new HttpClient())
            {
                booking b = new booking()
                {
                    id_room = int.Parse(test.Get("room")),
                    email_user = HttpContext.User.Identity.Name,
                    date = DateTime.Parse(test.Get("day")),
                    begin_time = TimeSpan.Parse(test.Get("start")),
                    end_time = TimeSpan.Parse(test.Get("endtime")),
                    equipment = test.Get("equipment") == "on"? true : false,
                };
                var req = conn.GetAsync(@"https://localhost:44360/api/values/" + b.email_user);
                req.Wait();
                string sub = "";
                if (req.Result.IsSuccessStatusCode)
                {
                    var subscribed = req.Result.Content.ReadAsStringAsync();
                    subscribed.Wait();
                    sub = subscribed.Result;
                    if (sub == "0") return RedirectToAction("Index");
                } else
                {
                    return RedirectToAction("Index");
                }
                b.price = (decimal)((sub == "\"False\"" ?  (interval.TotalMinutes / 30) * 7 : 0) + (!b.equipment ? 3 : 0));
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
            ViewBag.Msg = TempData["msg"];
            List<booking> ls = new List<booking>();
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync(@"https://localhost:44360/api/values/bookingsperuser/" + HttpContext.User.Identity.Name);
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
        [HttpGet]
        public ActionResult Delete(int id)
        {            
            using (var conn = new HttpClient())
            {
                var req = conn.DeleteAsync(@"https://localhost:44360/api/values/" + HttpContext.User.Identity.Name + "/" + id);
                req.Wait();
                var res = req.Result;
                var t = res.Content.ReadAsStringAsync();
                t.Wait();
                TempData["msg"] = t.Result.Equals("1") ? "Delete Successfull" : "Delete wasn't performed.";
            }
            return RedirectToAction("DeleteBooking");
        }

        public ActionResult UpdateBooking()
        {
            ViewBag.Msg = TempData["msg"];
            List<booking> ls = new List<booking>();
            using (var conn = new HttpClient())
            {
                var req2 = conn.GetAsync("https://localhost:44360/api/values/getrooms");
                req2.Wait();
                var res2 = req2.Result;
                if (res2.IsSuccessStatusCode)
                {
                    var data = res2.Content.ReadAsStringAsync();
                    data.Wait();
                    ViewBag.Rooms = JsonConvert.DeserializeObject<List<room>>(data.Result);
                }
                var req = conn.GetAsync(@"https://localhost:44360/api/values/bookingsperuser/" + HttpContext.User.Identity.Name);
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
        public ActionResult Update()
        {
            var test = HttpContext.Request.Form;
            TimeSpan interval;
            if (ValidationData(test).Equals(new TimeSpan()))
            {
                TempData["msg"] = "You tried.";
                return RedirectToAction("UpdateBooking");
            }
            else
            {
                interval = ValidationData(test);
            }
            using (var conn = new HttpClient())
            {
                booking b = new booking()
                {
                    id = int.Parse(test.Get("id")),
                    id_room = int.Parse(test.Get("room")),
                    email_user = HttpContext.User.Identity.Name,
                    date = DateTime.Parse(test.Get("day")),
                    begin_time = TimeSpan.Parse(test.Get("start")),
                    end_time = TimeSpan.Parse(test.Get("endtime")),
                    equipment = test.Get("equipment") == "on" ? true : false
                };
                var req = conn.GetAsync(@"https://localhost:44360/api/values/" + b.email_user);
                req.Wait();
                string sub = "";
                if (req.Result.IsSuccessStatusCode)
                {
                    var subscribed = req.Result.Content.ReadAsStringAsync();
                    subscribed.Wait();
                    sub = subscribed.Result;
                    if (sub == "0") return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("UpdateBooking");
                }
                b.price = (decimal)((sub == "\"False\"" ? (interval.TotalMinutes / 30) * 7 : 0) + (!b.equipment ? 3 : 0));
                StringContent content = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
                var req2 = conn.PutAsync(@"https://localhost:44360/api/values/updatebooking", content);
                req2.Wait();
                TempData["msg"] = req2.Result.IsSuccessStatusCode ? "Successfully Booked. Total Price is: " + String.Format("{0:C}", b.price)
                                                                    : "I'm sorry, your booking wasn't confirmed.";
            }

            return RedirectToAction("UpdateBooking");

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
        protected TimeSpan ValidationData(NameValueCollection test)
        {
            if (test.Get("room") is null || test.Get("start") is null || test.Get("endtime") is null ||
                (DateTime.Parse(test.Get("day")) < DateTime.Now
                 && TimeSpan.Parse(test.Get("start")) < DateTime.Now.TimeOfDay)
               )
            {
                TempData["msg"] = "Error posting your booking, please check your data.";
                return new TimeSpan();
            }
            TimeSpan interval = TimeSpan.Parse(test.Get("endtime")) - TimeSpan.Parse(test.Get("start"));
            if (interval.TotalMinutes <= 0)
            {
                TempData["msg"] = "Error posting your booking, please check your data.";
                return new TimeSpan();
            }
            return interval;
        }
    }
}