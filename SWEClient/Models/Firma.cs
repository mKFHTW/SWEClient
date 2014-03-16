using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEClient.Models
{
    class Firma
    {
        public int ID { get; set; }
        public string UID { get; set; }
        public string Name { get; set; }
        public string Adresse { get; set; }
        public int PLZ { get; set; }
        public string Ort { get; set; }
    }
}
