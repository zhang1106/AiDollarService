using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Compliance.ApiGateway.Models
{
    public class Execution
    {
        public string TradeId { get; set; }
        public long Quantity { get; set; }
        public DateTime ExecutionTime { get; set; }
        public decimal Price { get; set; }
        public string Exchange { get; set; }
    }
}
