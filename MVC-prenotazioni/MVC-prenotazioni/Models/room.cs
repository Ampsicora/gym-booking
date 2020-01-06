using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_prenotazioni.Models
{
    public class room
    {
        public room()
        {
        }

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

    }
}