using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEClient.Models
{
    class InputRechnung
    {
        public int KundenID { get; set; }
        public string Kundenname { get; set; }
        public string Kommentar { get; set; }
        public string Nachricht { get; set; }
        public DateTime Due { get; set; }
        public int Stk { get; set; }
        public string Artikel { get; set; }
        public int Preis { get; set; }
    }
}
