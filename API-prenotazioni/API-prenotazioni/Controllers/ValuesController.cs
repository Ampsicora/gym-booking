using API_prenotazioni.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_prenotazioni.Controllers
{
    public class ValuesController : ApiController
    {
        [Route("api/values")]
        public List<booking> Get()
        {
            List<Booking> res;
            using (var db = new palestraEntities())
            {
                res = db.Booking.OrderBy(p => p.date).ThenBy(p => p.begin_time).ToList();

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Booking, booking>();
                    cfg.CreateMap<Room, room>();
                    cfg.CreateMap<User, user>();
                });

                var mapper = new Mapper(config);

                List<booking> bookings = mapper.Map<List<Booking>, List<booking>>(res);

                return bookings;
            }
        }
        [Route("api/values/{mail}")]
        [HttpGet]
        public string IsUserSubscribed(string mail)
        {
            using (var db = new palestraEntities())
            {
                var res = db.User.Where(p => p.email.Equals(mail)).FirstOrDefault();
                return res != null ? res.subscribed.ToString() : "0";
            }
        }

        [Route("api/values/bookingsperuser/{mail}")]
        [HttpGet]
        public List<booking> Get(string mail)
        {
            List<Booking> res;
            using (var db = new palestraEntities())
            {
                res = db.Booking.Where(p => p.email_user == mail)
                    .OrderBy(p => p.date).ThenBy(p => p.begin_time)
                    .ToList();

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Booking, booking>();
                    cfg.CreateMap<Room, room>();
                    cfg.CreateMap<User, user>();
                });

                var mapper = new Mapper(config);

                List<booking> bookings = mapper.Map<List<Booking>, List<booking>>(res);

                return bookings;
            }
        }

        [Route("api/values/getrooms")]
        public List<room> GetRooms()
        {
            List<Room> res;
            using (var db = new palestraEntities())
            {
                res = db.Room.ToList();

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Room, room>();
                });

                var mapper = new Mapper(config);

                List<room> bookings = mapper.Map<List<Room>, List<room>>(res);

                return bookings;
            }
        }

        [Route("api/values/newbooking")]
        public HttpResponseMessage Post([FromBody]booking b)
        {
            using (var db = new palestraEntities())
            {
                var res = db.Booking.Where(p =>
                                           p.id_room == b.id_room
                                           && p.date.Equals(b.date)
                                           && p.begin_time < b.end_time
                                           && p.end_time > b.begin_time)
                                           .FirstOrDefault();
                if (res != null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
                db.Booking.Add(new Booking()
                {
                    id_room = b.id_room,
                    email_user = b.email_user,
                    date = b.date,
                    begin_time = b.begin_time,
                    end_time = b.end_time,
                    equipment = b.equipment,
                    price = b.price
                });
                db.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

        }
        [Route("api/values/updatebooking")]

        public HttpResponseMessage Put([FromBody]booking b)
        {
            using (var db = new palestraEntities())
            {
                var res2 = db.Booking.Where(p => p.email_user.Equals(b.email_user) && p.id == b.id).FirstOrDefault();
                if (res2 == null) return new HttpResponseMessage(HttpStatusCode.NotFound);
                var res = db.Booking.Where(p =>
                                           p.id != b.id
                                           && p.id_room == b.id_room
                                           && p.date.Equals(b.date)
                                           && p.begin_time < b.end_time
                                           && p.end_time > b.begin_time)
                                           .FirstOrDefault();
                if (res != null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }


                res2.id_room = b.id_room;
                res2.date = b.date;
                res2.begin_time = b.begin_time;
                res2.end_time = b.end_time;
                res2.equipment = b.equipment;
                res2.price = b.price;
                db.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        [Route("api/values/{mail}/{id}")]
        public int Delete(string mail, int id)
        {
            using (var db = new palestraEntities())
            {
                var res = db.Booking.Where(p => p.email_user.Equals(mail) && p.id == id).FirstOrDefault();

                if (res == null)
                    return 0;

                else
                    db.Booking.Remove(res);

                return db.SaveChanges();
            }
        }
    }
}
