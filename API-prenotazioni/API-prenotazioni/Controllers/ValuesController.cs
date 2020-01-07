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
                res = db.Booking.OrderBy(p => p.date).ToList();
                
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



        // GET api/values/bookingsperuser/mail => get all bookings per user
        [Route("api/values/bookingsperuser/{mail}")]
        [HttpGet]
        public List<booking> Get(string mail)
        {
            List<Booking> res;
            using (var db = new palestraEntities())
            {
                res = db.Booking.Where(p => p.email_user == mail).ToList();

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

        // POST api/values
        [Route("api/values/newbooking")]
        public void Post([FromBody]booking b)
        {
            var bs = b;

        }
        [Route("api/values/updatebooking")]

        // PUT api/values/5
        public void Put([FromBody]booking b)
        {
        }

        // DELETE api/values/5
        [Route("api/values/{mail}/{id}")]
        public void Delete(int id)
        {
        }
    }
}
