using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Bam.Compliance.ApiGateway.Models;
using Bam.Compliance.ApiGateway.Services;

namespace Bam.Compliance.ApiGateway.Http.Controller
{
    public class TradeController : ApiController
    {
        private readonly IHistoricTradeRetriever _historicTradeRetriever;
        private readonly IIntradayAggrRetriever _intradayTradeRetriever;
        private readonly IExecutionRetriever _executionRetriever;

        public TradeController(IHistoricTradeRetriever historicTradeRetriever, IIntradayAggrRetriever intradayRetriever, IExecutionRetriever executionRetriever )
        {
            _historicTradeRetriever = historicTradeRetriever;
            _executionRetriever = executionRetriever;
            _intradayTradeRetriever = intradayRetriever;
        }

        [AllowAnonymous]
        public List<Trade> GetHistoricTrades(string bamSymbol, DateTime start, DateTime end)
        {
            var trades = _historicTradeRetriever.GetTrades(bamSymbol, start, end).ToList();
            return trades;
        }

        [AllowAnonymous]
        public List<Trade> GetIntradayTrades(string bamSymbol)
        {
            var trades = _intradayTradeRetriever.GetTrades(bamSymbol).ToList();
            return trades;
        }

        [AllowAnonymous]
        public List<Execution> GetExecutions(string tradeId)
        {
            var executions = _executionRetriever.GetExecutions(tradeId).ToList();
            return executions;
        }

    }
}
