using System;
using System.Collections.Generic;

namespace AiDollar.Edgar.Model
{
    public class AiPortfolio
    {
        public string Cik { get; set; }
        public string Owner { get; set; }
        public string Fund { get; set; }
        public DateTime ReportedDate0 { get; set; }
        public DateTime ReportedDate1 { get; set; }
        public DateTime ReportedDate2 { get; set; }
        public DateTime ReportedDate3 { get; set; }
        public DateTime ReportedDate4 { get; set; }
        public IList<AiHolding> Holdings { get; set; }
    }

    public struct AiHolding
    {
        public string Issuer { get; set; }
        public string Ticker { get; set; }
        public string Cusip { get; set; }
        public long Share0 { get; set; }
        public long Share1 { get; set; }
        public long Share2 { get; set; }
        public long Share3 { get; set; }
        public long Share4 { get; set; }
    }
}

     