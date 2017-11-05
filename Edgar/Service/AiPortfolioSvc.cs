using System;
using System.Linq;
using AiDollar.Edgar.Model;

namespace AiDollar.Edgar.Service
{
    public class AiPortfolioSvc:IAiPortfolioSvc
    {
        private readonly IEdgarApi _edgarApi;
        private readonly ILookup<string, Security> _securities;

        public AiPortfolioSvc(IEdgarApi edgarApi)
        {
            _edgarApi = edgarApi;
            _securities = _edgarApi.GetSecurities().ToLookup(s=>s.GetCusip(), s=>s);
        }

        public Security GetSecurity(string cusip)
        {
            return _securities.Contains(cusip)? _securities[cusip].OrderBy(s=>s.Ticker.Length).FirstOrDefault():null;
        }

        public AiPortfolio GetPortfolio(string cik)
        {
            Console.WriteLine($"Calc portfolio for {cik}");
            var portfolios = _edgarApi.GetPortfolios(cik);
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
            portfolio.Holdings = aiPort.ToList();

            return portfolio;
        }
    } 
}
