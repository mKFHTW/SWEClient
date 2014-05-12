using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEClient.Models
{
    class SearchReceipt
    {
        public string BetragVon { get; set; }
        public string BetragBis { get; set; }
        public DateTime DateVon { get; set; }
        public DateTime DateBis { get; set; }
        public string Name { get; set; }
    }
}
