using System.Collections.Generic;
using Bam.Compliance.ApiGateway.Models;

namespace Bam.Compliance.ApiGateway.Services
{
    public interface  IAiPortfolioSvc
    {
        IList<AiPortfolio> GetPortfolios(string cik);
    }
}
