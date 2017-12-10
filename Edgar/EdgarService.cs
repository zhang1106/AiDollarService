using System.Linq;
using AiDollar.Edgar.Service.Service;
using AiDollar.Infrastructure.Hosting;
using Newtonsoft.Json;

namespace AiDollar.Edgar.Service
{
    public class EdgarService:IService
    {
        private readonly IF4Svc _f4Svc;
        private readonly int _f4InDays;
        private readonly string _outputPath;
        private readonly IUtil _util;

        public EdgarService(IF4Svc f4Svc, IUtil util, int f4InDays, string outputPath)
        {
            _f4Svc = f4Svc;
            _f4InDays = f4InDays;
            _outputPath = outputPath;
            _util = util;
        }

        public void Start()
        {
           var trades = _f4Svc.DownloadLatestInsideTrades(3);
           _util.WriteToDisk(_outputPath+"f4trdes.json", JsonConvert.SerializeObject(trades));
           GenerateF4();
        }

        private void GenerateF4()
        {
            var trades = _f4Svc.GetLatestInsideTrades(_f4InDays)
                .Select(f=>new{f.Issuer,f.Amount,f.Reporter,f.RemainAmount,f.Price,f.Symbol,f.Role,f.TransactionDate,f.TransactionCode})
                .Distinct()
                .OrderByDescending(f=>f.Issuer);
            _util.WriteToDisk(_outputPath+"f4.json", JsonConvert.SerializeObject(trades));
        }
       
        public void Stop()
        { 
        }

        public string Name => "Edgar Service";
    }
}
