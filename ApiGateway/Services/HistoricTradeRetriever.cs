using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using Bam.Compliance.ApiGateway.Models;
using Bam.Compliance.Infrastructure.Database;

namespace Bam.Compliance.ApiGateway.Services
{
    public class HistoricTradeRetriever : IHistoricTradeRetriever
    {
        private const string Query = @" SELECT 
                                               [tradedate]
                                              , Fund = [Fundcode]
                                              , Strategy = [StrategyCode]
                                              , Trader = [EB_T_TRADER]
                                              , Side = [EventType]
                                              ,[Quantity]
                                              , BamSymbol = [InvestmentCode]
                                              , Broker = [BrokerCode]
                                              , TradeId = isnull(Convert(varchar(50),[EA_T_TradeID]),[UserTranID1])
                                        FROM bamcorelite.txn.TxnAll(nolock)
                                        WHERE InvestmentCode like '{0}'
                                        And TxnStatus<> 'Deleted'
                                        And EventType in ('SellShort', 'Sell', 'CoverShort', 'Buy')
                                        And TradeDate >= '{1}'
                                        And TradeDate <= '{2}'";

        private readonly string _connectionString;
        private const string _flexId = @"FLEX_.*-.*";
        private Regex _flexIdPatten;

        public HistoricTradeRetriever(string connectionString)
        {
            _connectionString = connectionString;
            _flexIdPatten = new Regex(_flexId, RegexOptions.IgnoreCase);
        }

        public IEnumerable<Trade> GetTrades(string bamSymbol, DateTime start, DateTime end)
        {
            var query = string.Format(Query, bamSymbol, start, end);
            using (var connection = new SqlConnection(_connectionString))
            {
                var dbOps = new DbOperation(connection);
                var trades = dbOps.Select<AllocatedTrade>(query).Select(t => new AllocatedTrade()
                {
                    TradeDate = t.TradeDate,
                    Fund = t.Fund,
                    Strategy = t.Strategy,
                    Trader = t.Trader,
                    Side = t.Side,
                    Quantity = t.Quantity,
                    BamSymbol = t.BamSymbol,
                    Broker = t.Broker,
                    TradeId = ParseId(t.TradeId)
                });
                return Util.Consolidate(trades);
            }
        }

        protected string ParseId(string original)
        {
            return !_flexIdPatten.IsMatch(original) ? original.TrimEnd() : original.Split('-')[0].Split('_')[1];
        }
    }
}
