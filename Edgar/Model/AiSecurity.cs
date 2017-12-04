using System.Collections.Generic;

namespace AiDollar.Edgar.Service.Model
{
    public class AiSecurityHoldingUnit
    {
        public string Owner { get; set; }
        public string Cik { get; set; }
        public long Recent { get; set; }
        public long Q1 { get; set; }
        public long Q2 { get; set; }
        public long Q3 { get; set; }
        public long Q4 { get; set; }

    }

    public class AiSecurityHolding
    {
        public string Ticker { get; set; }
        public string Cusip { get; set; }
        public string Issuer { get; set; }
        public IList<AiSecurityHoldingUnit> Holding { get; set; }
    }
}
