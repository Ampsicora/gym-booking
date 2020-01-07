using API_prenotazioni.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;

namespace API_prenotazioni.Controllers
{
    public class UserController : ApiController
    {

        public string Get()
        {
            return "ok";
        }
        // GET: api/User/5
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/user/{mail}/{password}")]
        public HttpResponseMessage Login(string mail, string password)
        {
            using (var db = new palestraEntities())
            {
                User res = db.User.Where(x => x.email.Equals(mail) && x.password.Equals(password)).FirstOrDefault();
                if (res != null)
                {
                    var x = new HttpResponseMessage();
                    x.StatusCode = HttpStatusCode.OK;
                    x.Content = new StringContent(res.subscribed.ToString(), Encoding.UTF8, "application/json");
                    return x;
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/user/register")]
        public HttpResponseMessage Register([FromBody] user u)
        {
            using (var db = new palestraEntities())
            {
                User res = db.User.Where(x => x.email.Equals(u.email)).FirstOrDefault();
                if (res == null)
                {
                    db.User.Add(new User()
                    {
                        email = u.email,
                        name = u.name,
                        surname = u.surname,
                        password = u.password,
                        birthday = u.birthday,
                        subscribed = u.subscribed
                    });
                    db.SaveChanges();
                    return new HttpResponseMessage(HttpStatusCode.OK);
                } else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }

        }


    }
}
