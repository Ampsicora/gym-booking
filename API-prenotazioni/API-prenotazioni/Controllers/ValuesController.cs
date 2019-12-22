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
        public List<booking> Get()
        {
            List<Booking> res;
            using (var db = new palestraEntities())
            {
                res = db.Booking.ToList();
                
                var config = new MapperConfiguration(cfg => 
                {
                    cfg.CreateMap<Booking, booking>();
                });

                var mapper = new Mapper(config);

                List<booking> bookings = mapper.Map<List<Booking>, List<booking>>(res);

                return bookings;
            }
        }



        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
