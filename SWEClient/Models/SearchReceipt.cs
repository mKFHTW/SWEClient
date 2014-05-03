using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEClient.Models
{
    class SearchReceipt
    {
        public int BetragVon { get; set; }
        public int BetragBis { get; set; }
        public DateTime DateVon { get; set; }
        public DateTime DateBis { get; set; }
        public string Name { get; set; }
    }
}
