using System.Web.Http;
using AiDollar.Edgar.Model;
using AiDollar.Edgar.Service;

namespace AiDollar.ApiGateway.Http.Controller
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
        public AiPortfolio GetPortfolio(string cik)
        {
            return _aiPortfolioSvc.GetPortfolio(cik);
        }
    }
}
