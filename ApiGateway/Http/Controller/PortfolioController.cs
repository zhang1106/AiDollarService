using System.Collections.Generic;
using System.Web.Http;
using AiDollar.Edgar.Service;

namespace Bam.Compliance.ApiGateway.Http.Controller
{
    public class PortfolioController:ApiController
    {
        private readonly IEdgarApi _edgarApi;

        public PortfolioController(IEdgarApi edgarApi)
        {
            _edgarApi = edgarApi;
        }

        [AllowAnonymous]
        [HttpGet]
        public string Whatup()
        {
            return "Hello";
        }

        [AllowAnonymous]
        [HttpGet]
        public IList<Portfolio> GetPortfolios(string cik)
        {
            return _edgarApi.GetPortfolios(cik);
        }
    }
}
