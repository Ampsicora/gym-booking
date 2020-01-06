using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_prenotazioni.Models
{
    public class room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public room()
        {
        }

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}