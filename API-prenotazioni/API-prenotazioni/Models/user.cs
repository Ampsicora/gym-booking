using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_prenotazioni.Models
{
    public class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
        }

        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public System.DateTime birthday { get; set; }
        public bool subscribed { get; set; }

    }
}