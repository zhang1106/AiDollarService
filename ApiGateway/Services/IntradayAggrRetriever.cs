using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Compliance.ApiGateway.Models;

namespace Bam.Compliance.ApiGateway.Services
{
    public class IntradayAggrRetriever : IIntradayAggrRetriever
    {
      
        private readonly IList<IIntradayRetriever> _retrievers;

        public IntradayAggrRetriever(IEnumerable<string> baseUrls)
        {
            _retrievers = new List<IIntradayRetriever>();
            foreach (var uri in baseUrls)
            {
                _retrievers.Add(new IntradayRetriever(uri));
            }
        }
        public IEnumerable<Trade> GetTrades(string bamSymbol)
        {
            var trades = new List<Trade>();
            foreach (var r in _retrievers)
            {
                trades.AddRange(r.GetTrades(bamSymbol));
            }

            return trades;
        }

        public IEnumerable<Position> GetPositions(string bamSymbol)
        {
            var positions = new List<Position>();
            foreach (var r in _retrievers)
            {
                positions.AddRange(r.GetPositions(bamSymbol));
            }

            return positions;
        }
    }
}
