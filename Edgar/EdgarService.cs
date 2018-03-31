using System.Linq;
using AiDollar.Edgar.Service.Service;
using AiDollar.Infrastructure.Hosting;
using Newtonsoft.Json;

namespace AiDollar.Edgar.Service
{
    public class EdgarService:IService
    {
        private readonly IF4Svc _f4Svc;
        private readonly IF13Svc _f13Svc;
        private readonly int _f4InDays;
        private readonly string _outputPath;
        private readonly IUtil _util;

        public EdgarService(IF4Svc f4Svc, IF13Svc f13Svc, IUtil util, int f4InDays, string outputPath)
        {
            _f4Svc = f4Svc;
            _f13Svc = f13Svc;
            _f4InDays = f4InDays;
            _outputPath = outputPath;
            _util = util;
        }

        public void Start()
        {
           // generate F4
           //download last three days inside trades and save to db if not saved yet.
           //return the new ones
           var trades = _f4Svc.DownloadLatestInsideTrades(3);
            //validation
            _util.WriteToDisk(_outputPath + "f4trdes.json", JsonConvert.SerializeObject(trades));

            //get last 3 days f4 trades from db
            //output the data to the publish folder
            GenerateF4();

            //generate f13
            //download latest f13 for gurus
            //save the data to database
            //output the data to the publish folder.
            //GenerateF13();
        }

        private void GenerateF13()
        {
            _f13Svc.GetLatest13F(_outputPath,false);
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
