using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEClient.Models
{
    public abstract class Kontakt
    {
        public string ID { get; set; }     
        public string Adresse { get; set; }
        public string PLZ { get; set; }
        public string Ort { get; set; }
    }
}
