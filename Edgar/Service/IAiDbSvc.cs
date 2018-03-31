using System.Collections.Generic;
using AiDollar.Edgar.Model;
using AiDollar.Edgar.Service.Model;

namespace AiDollar.Edgar.Service
{
    public interface IAiDbSvc
    {
        IEnumerable<Portfolio> GetPortfolios(string cik);
        IEnumerable<Security> GetSecurities();
        IEnumerable<Guru> GetGurus();
        IEnumerable<InsideTrade> GetInsideTrades();
    }
}
