using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiDollar.Edgar.Service.Model
{
    public class Guru
    {
        public object _id { get; set; }
        public string Owner { get; set; }
        public string Fund { get; set; }
        public int Cik { get; set; }
        public int Rank { get; set; }
    }
}
