using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Compliance.ApiGateway.Models
{
    public class Position
    {
        public long PositionId { get; set; }
        public string Portfolio { get; set; }
        public string BamSymbol { get; set; }
        public long ActualQuantity { get; set; }
        public long TheoreticalQuantity { get; set; }
        public long ShortMarkingQuantity { get; set; }
        public long LongMarkingQuantity { get; set; }
        public DateTime LastUpdated { get; set; }

        public IReadOnlyList<ActualPositionAllocation> ActualAllocations { get; set; }

        public IReadOnlyList<TheoreticalPositionAllocation> TheoreticalAllocations { get; set; }

        public struct ActualPositionAllocation
        {
            public string Fund { get; set; }
            public string Custodian { get; set; }
            public long Quantity { get; set; }
        }

        public struct TheoreticalPositionAllocation
        {
            public string Custodian{ get; set; }
            public long Quantity { get; set; }
        }
    }
}
