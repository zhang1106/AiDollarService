using System;
using System.Collections.Generic;

namespace Bam.Compliance.ApiGateway.Models
{
    public class AiPortfolio
    {
        public string Issuer { get; set; }
        public IList<TShare> Shares { get; set; }
    }

    public struct TShare
    {
        public long Share;
        public DateTime Updated;
    }
}
