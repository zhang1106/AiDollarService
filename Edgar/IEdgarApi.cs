using System.Collections.Generic;

namespace AiDollar.Edgar.Service
{
    public interface IEdgarApi
    {
        IList<Portfolio> GetPortfolios(string cik);
    }
}
