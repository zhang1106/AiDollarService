using System;
using System.Linq;
using AiDollar.Edgar.Model;

namespace AiDollar.Edgar.Service
{
    public class AiPortfolioSvc:IAiPortfolioSvc
    {
        private readonly IAiDbSvc _aiDbSvc;
        private readonly ILookup<string, Security> _securities;

        public AiPortfolioSvc(IAiDbSvc aiDbSvc)
        {
            _aiDbSvc = aiDbSvc;
            _securities = _aiDbSvc.GetSecurities().ToLookup(s=>s.Cusip.TrimStart(new []{'0'}), s=>s);
        }

        public Security GetSecurity(string cusip)
        {
            var c = cusip.TrimStart(new[] {'0'});
            return _securities.Contains(c)? _securities[c].OrderBy(s=>s.Ticker.Length).FirstOrDefault():null;
        }

        public AiPortfolio GetPortfolio(string cik)
        {
            Console.WriteLine($"Calc portfolio for {cik}");
            var portfolios = _aiDbSvc.GetPortfolios(cik).ToList();
            if (!portfolios.Any()) return null;
            var entries = portfolios.SelectMany(p => 
                p.infoTable.GroupBy(g=>new{g.cusip, g.nameOfIssuer}).Select(g=>new { p.ReportedDate, cusip = g.Key,
                    Shares = g.Sum(i=>long.Parse(i.shrsOrPrnAmt.sshPrnamt))}))
            ;
            var dates = entries.Select(e => e.ReportedDate).Distinct().OrderByDescending(d=>d).ToList();
            var portfolio = new AiPortfolio()
            {
                Cik = cik,
                Owner = portfolios[0].Holder,
                ReportedDate4 = dates.Count>0?dates[0]:default(DateTime),
                ReportedDate3 = dates.Count > 1 ? dates[1] : default(DateTime),
                ReportedDate2 = dates.Count > 2 ? dates[2] : default(DateTime),
                ReportedDate1 = dates.Count > 3 ? dates[3] : default(DateTime),
                ReportedDate0 = dates.Count > 4 ? dates[4] : default(DateTime),
             };

            var aiPort = from e in entries
                group e by new {e.cusip}
                into g
                let h = g.OrderByDescending(a => a.ReportedDate).Select(k => k.Shares).ToList()
                select new AiHolding()
                {
                    Ticker = GetSecurity(g.Key.cusip.cusip)?.Ticker,
                    Issuer = g.Key.cusip.nameOfIssuer,
                    Cusip = g.Key.cusip.cusip,
                    Share0 = h.Count > 4 ? h[4] : 0,
                    Share1 = h.Count > 3 ? h[3] : 0,
                    Share2 = h.Count > 2 ? h[2] : 0,
                    Share3 = h.Count > 1 ? h[1] : 0,
                    Share4 = h.Count > 0 ? h[0] : 0
                };
            portfolio.Holdings = aiPort.OrderByDescending(h=>h.Share4).ToList();

            return portfolio;
        }
    } 
}
