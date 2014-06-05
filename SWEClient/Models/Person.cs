using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEClient.Models
{
    public class Person : Kontakt, ICloneable
    {        
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Titel { get; set; }
        public string Suffix { get; set; }
        public string Firm { get; set; }
        public string FirmaID { get; set; }
        public DateTime GebDatum { get; set; }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
