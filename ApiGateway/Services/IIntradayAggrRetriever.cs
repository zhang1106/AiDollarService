using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Compliance.ApiGateway.Models;

namespace Bam.Compliance.ApiGateway.Services
{
    public interface IIntradayAggrRetriever
    {
        IEnumerable<Trade> GetTrades(string bamSymbol);
        IEnumerable<Position> GetPositions(string bamSymbol);
    }
}
