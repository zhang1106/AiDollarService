using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Compliance.ApiGateway.Models
{
    public class Trade
    {
      public DateTime TradeDate { get; set; }
	  public string Strategy { get; set; }
	  public string Trader { get; set; }
	  public string Side { get; set; }
      public long Quantity { get; set; }
	  public string BamSymbol { get; set; }
	  public string Broker { get; set; }
      public string TradeId { get; set; }
      public IList<Allocation> Allocations { get; set; }
      public struct Allocation
      {
        public string Fund { get; set; }
        public decimal Quantity { get; set; }
      }
    }
}
