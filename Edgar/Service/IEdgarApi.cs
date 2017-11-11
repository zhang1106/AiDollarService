﻿using System.Collections.Generic;
using AiDollar.Edgar.Model;
using AiDollar.Edgar.Service.Model;

namespace AiDollar.Edgar.Service
{
    public interface IEdgarApi
    {
        IList<Portfolio> GetPortfolios(string cik);
        IList<Security> GetSecurities();
        IList<Guru> GetGurus();
    }
}