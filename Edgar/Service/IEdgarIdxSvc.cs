 using System.Collections.Generic;
 using AiDollar.Edgar.Service.Model;

namespace AiDollar.Edgar.Service.Service
{
    public interface IEdgarIdxSvc
    {
        IEnumerable<EdgarIdx> Parse(EdgarIdxDesc desc);
        
        EdgarIdxDesc DownLoadEdgarIdx();
    }
}
