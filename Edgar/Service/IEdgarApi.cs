using System.Collections.Generic;
using AiDollar.Edgar.Model;

namespace AiDollar.Edgar.Service
{
    public interface IEdgarApi
    {
        IList<Portfolio> GetPortfolios(string cik);
        IList<Security> GetSecurities();
    }
}
