using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Bam.Compliance.ApiGateway.Models;

namespace Bam.Compliance.ApiGateway.Services
{
    public class Util
    {
        public static IEnumerable<Trade> Consolidate(IEnumerable<AllocatedTrade> aTrades)
        {
            var groups = (from t in aTrades
                group t by new {t.TradeId, t.BamSymbol, t.Broker, t.Side, t.Strategy, t.TradeDate, t.Trader}
                into g
                select new Trade()
                {
                    TradeId = g.Key.TradeId,
                    TradeDate = g.Key.TradeDate,
                    Strategy = g.Key.Strategy,
                    Broker = g.Key.Broker,
                    Side = g.Key.Side,
                    BamSymbol = g.Key.BamSymbol,
                    Trader = g.Key.Trader,
                    Quantity = g.Sum(t => t.Quantity),
                }).ToList();

            foreach (var g in groups)
            {
                g.Allocations = aTrades.Where(a => a.TradeId == g.TradeId && a.Side==g.Side && a.Strategy==g.Strategy && a.TradeDate==g.TradeDate && a.Trader==g.Trader
                
                ).Select(a => new Trade.Allocation() {Fund=a.Fund, Quantity = a.Quantity}).ToList();
            }

            return groups.ToList();

        }
    }
}
