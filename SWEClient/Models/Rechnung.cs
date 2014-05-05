using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEClient.Models
{
    public class Rechnung
    {
        public int KundenID { get; set; }
        public string Kundenname { get; set; }
        public string Kommentar { get; set; }
        public string Nachricht { get; set; }
        public DateTime Due { get; set; }
        public List<Models.Rechnungszeile> Zeilen { get; set; }

        public Rechnung()
        {
            Zeilen = new List<Rechnungszeile>();
        }
    }
}
