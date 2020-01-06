using API_prenotazioni.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_prenotazioni.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/User
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/User/5
        [Route("api/user/{mail}/{password}")]
        public string Get(string mail, string password)
        {
            return "value";
        }
        [HttpPost]
        [Route("api/user/register")]
        public string Register([FromBody] user u)
        {
            return "ok";
        }


        // POST: api/User
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
