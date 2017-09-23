using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiDollar.Infrastructure.Hosting;

namespace AiDollar.Edgar.Service
{
    public class EdgarService:IService
    {
        private readonly IHttpDataAgent _httpDataAgent;

        public EdgarService(IHttpDataAgent httpDataAgent)
        {
            _httpDataAgent = httpDataAgent;
        }
        public void Start()
        {
            
        }

        public void Stop()
        { 
        }

        public string Name => "Edgar Service";
    }
}
