using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEClient.Models
{
    public class Firma : Kontakt, ICloneable
    {
        public string UID { get; set; }
        public string Name { get; set; }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
