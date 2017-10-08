using System.Collections.Generic;
using System.Web.Http;
using AiDollar.Edgar.Service;
using Bam.Compliance.ApiGateway.Models;
using Bam.Compliance.ApiGateway.Services;

namespace Bam.Compliance.ApiGateway.Http.Controller
{
    public class PortfolioController:ApiController
    {
        private readonly IAiPortfolioSvc _aiPortfolioSvc;
        public PortfolioController(IAiPortfolioSvc aiPortfolioSvc)
        {
            _aiPortfolioSvc = aiPortfolioSvc;
        }

        [AllowAnonymous]
        [HttpGet]
        public string Whatup()
        {
            return "Hello";
        }

        [AllowAnonymous]
        [HttpGet]
        public IList<AiPortfolio> GetPortfolios(string cik)
        {
            return _aiPortfolioSvc.GetPortfolios(cik);
        }
    }
}
