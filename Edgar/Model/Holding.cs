using System;
using System.Collections.Generic;

namespace AiDollar.Edgar.Model
{
    public class ShrsOrPrnAmt
    {
        public string sshPrnamt { get; set; }
        public string sshPrnamtType { get; set; }
    }

    public class VotingAuthority
    {
        public string Sole { get; set; }
        public string Shared { get; set; }
        public string None { get; set; }
    }

    public class InfoTable
    {
        public string nameOfIssuer { get; set; }
        public string titleOfClass { get; set; }
        public string cusip { get; set; }
        public string value { get; set; }
        public ShrsOrPrnAmt shrsOrPrnAmt { get; set; }
        public string investmentDiscretion { get; set; }
        public string otherManager { get; set; }
        public VotingAuthority votingAuthority { get; set; }
    }

    public class Holding
    {
        public List<InfoTable> infoTable { get; set; }
    }

    public class HoldingRoot
    {
        public Holding holding { get; set; }
    }

    public class Portfolio
    {
        public object _id { get; set; }
        public List<InfoTable> infoTable { get; set; }
        public DateTime ReportedDate { get; set; }
        public string Cik { get; set; }
        public string Holder { get; set; }
    }
}
