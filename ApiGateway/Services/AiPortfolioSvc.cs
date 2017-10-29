using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AiDollar.Edgar.Service;
using Bam.Compliance.ApiGateway.Models;

namespace Bam.Compliance.ApiGateway.Services
{
    public class AiPortfolioSvc:IAiPortfolioSvc
    {
        private readonly IEdgarApi _edgarApi;

        public AiPortfolioSvc(IEdgarApi edgarApi)
        {
            _edgarApi = edgarApi;
        }
        public AiPortfolio GetPortfolio(string cik)
        {
            var portfolios = _edgarApi.GetPortfolios(cik);
            var entries = portfolios.SelectMany(p => 
                p.infoTable.GroupBy(g=>g.nameOfIssuer).Select(g=>new { p.ReportedDate, Issuer = g.Key,
                    Shares = g.Sum(i=>long.Parse(i.shrsOrPrnAmt.sshPrnamt))}))
            ;
            var dates = entries.Select(e => e.ReportedDate).Distinct().OrderByDescending(d=>d).ToList();
            var portfolio = new AiPortfolio()
            {
                Owner = portfolios[0].Holder,
                ReportedDate4 = dates.Count>0?dates[0]:default(DateTime),
                ReportedDate3 = dates.Count > 1 ? dates[1] : default(DateTime),
                ReportedDate2 = dates.Count > 2 ? dates[2] : default(DateTime),
                ReportedDate1 = dates.Count > 3 ? dates[3] : default(DateTime),
                ReportedDate0 = dates.Count > 4 ? dates[4] : default(DateTime),
             };

            var aiPort = from e in entries
                group e by new {e.Issuer}
                into g
                let h = g.OrderByDescending(a => a.ReportedDate).Select(k => k.Shares).ToList()
                select new AiHolding()
                {
                    Issuer = g.Key.Issuer,
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
