using API_prenotazioni.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace API_prenotazioni.Controllers
{
    /// <summary>
    /// User operations Controller.
    /// </summary>
    public class UserController : ApiController
    {
        /// <summary>
        /// Check if user login data are correct.
        /// </summary>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/user/{mail}/{password}")]
        public HttpResponseMessage Login(string mail, string password)
        {
            using (var db = new palestraEntities())
            {
                string hash = HashPassword(password);
                User res = db.User.Where(x => x.email.Equals(mail) && x.password.Equals(hash)).FirstOrDefault();
                if (res != null)
                {
                    var x = new HttpResponseMessage();
                    x.StatusCode = HttpStatusCode.OK;
                    x.Content = new StringContent(res.subscribed.ToString(), Encoding.UTF8, "application/json");
                    return x;
                }
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
        /// <summary>
        /// Register, if possible, a new user.
        /// </summary>
        [HttpPost]
        [Route("api/user/register")]
        public HttpResponseMessage Register([FromBody] user u)
        {
            using (var db = new palestraEntities())
            {
                User res = db.User.Where(x => x.email.Equals(u.email)).FirstOrDefault();
                if (res == null)
                {
                    string hash = HashPassword(u.password);
                    db.User.Add(new User()
                    {
                        email = u.email,
                        name = u.name,
                        surname = u.surname,
                        password = hash,
                        birthday = u.birthday,
                        subscribed = u.subscribed
                    });
                    db.SaveChanges();
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
        /// <summary>
        /// Hash a given password.
        /// <param name="password">Password given.</param>
        /// </summary>
        protected string HashPassword(string password)
        {
            byte[] data = Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return Encoding.ASCII.GetString(data);
        }
    }
}
