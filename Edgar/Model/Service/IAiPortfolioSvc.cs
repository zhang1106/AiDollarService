using AiDollar.Edgar.Model;

namespace AiDollar.Edgar.Service
{
    public interface  IAiPortfolioSvc
    {
        AiPortfolio GetPortfolio(string cik);
    }
}
