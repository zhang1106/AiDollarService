 
using System;
using System.Collections.Generic;
using Bam.Compliance.ApiGateway.Models;

namespace Bam.Compliance.ApiGateway.Services
{
    public interface IHistoricTradeRetriever
    {
        IEnumerable<Trade> GetTrades(string bamSymbol, DateTime start, DateTime end);
    }
}
