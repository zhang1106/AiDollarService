using System.Collections.Generic;
using AiDollar.Edgar.Service.Model;

namespace AiDollar.Edgar.Service.Service
{
    public interface IF4Svc
    {
       IEnumerable<InsideTrade> DownloadLatestInsideTrades(int days);
   
    }
}
