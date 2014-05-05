using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEClient.Models
{
    public class Rechnungszeile
    {
        public int Stk { get; set; }
        public string Artikel { get; set; }
        public int Preis { get; set; }
    }
}
