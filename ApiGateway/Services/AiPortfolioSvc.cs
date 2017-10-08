using System.Collections.Generic;
using System.Linq;
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
        public IList<AiPortfolio> GetPortfolios(string cik)
        {
            var portfolios = _edgarApi.GetPortfolios(cik).SelectMany(p => 
                p.infoTable.GroupBy(g=>g.nameOfIssuer).Select(g=>new { p.ReportedDate, Issuer = g.Key,
                    Shares = g.Sum(i=>long.Parse(i.shrsOrPrnAmt.sshPrnamt))}))
            ;

            var aiPortf = portfolios.GroupBy(p=>new{p.Issuer}).Select(a => new AiPortfolio()
                {
                    Issuer = a.Key.Issuer,
                    Shares = a.Select(g=>new TShare(){Updated=g.ReportedDate, Share = g.Shares}).ToList()
                }
            );

            return aiPortf.ToList();
        }
    } 
}
