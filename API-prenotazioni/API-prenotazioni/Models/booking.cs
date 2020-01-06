using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_prenotazioni.Models
{
    public class booking
    {
        public int id { get; set; }
        public int id_room { get; set; }
        public string email_user { get; set; }
        public System.DateTime date { get; set; }
        public System.TimeSpan begin_time { get; set; }
        public System.TimeSpan end_time { get; set; }
        public bool equipment { get; set; }
        public decimal price { get; set; }
        public virtual room Room { get; set; }
        public virtual user User { get; set; }

    }
}