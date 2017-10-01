using AiDollar.Infrastructure.Database;
using AiDollar.Infrastructure.Hosting;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace AiDollar.Edgar.Service
{
    public class EdgarService:IService
    {
        private readonly IHttpDataAgent _httpDataAgent;
        private readonly IUtil _util;
        private readonly string[] _ciks;
        private readonly string _edgarUri;
        private readonly string _outputPath;
        private readonly string _posPage;
        private readonly IDbOperation _dbOperation;

        public EdgarService(IHttpDataAgent httpDataAgent, IUtil util, IDbOperation dbOperation, string[] ciks, string edgarUri, string posPage, 
            string outputPath)
        {
            _httpDataAgent = httpDataAgent;
            _util = util;
            _edgarUri = edgarUri;
            _ciks = ciks;
            _outputPath = outputPath;
            _posPage = posPage;
            _dbOperation = dbOperation;
        }
        public void Start()
        {
            foreach (var cik in _ciks)
            {
                try
                {
                    var uri = string.Format(_edgarUri, cik);
                    var doc = _httpDataAgent.DownloadXml(uri);
               
                    var json = _util.ToJson(_util.GetSpecialXmlElements("root", new []{"company-info","entry"}, doc));

                 
                    var root = JsonConvert.DeserializeObject<RootObject>(json);
                    var entry = root.root.entry.OrderByDescending(e => e.updated).FirstOrDefault();
                    var posIdxPage = entry.content.accession_nunber + "-index.htm";
                    var link = entry.link.href.Replace(posIdxPage, _posPage);
                    

                    var posFile = _outputPath + "holding-" + cik + "-" + entry.content.accession_nunber + ".json";
                    var jPos = DownloadLatestPosition(link, posFile);

                    var holding = JsonConvert.DeserializeObject<HoldingRoot>(jPos);
                    var holding13 = new Portfolio()
                    {
                        infoTable = holding.holding.infoTable,
                        Cik = cik,
                        ReportedDate = entry.updated,
                        Holder = root.root.company_info.conformed_name
                    };

                    _dbOperation.SaveItems(new[] {holding13}, "Portfolio");
                 
                    _util.WriteToDisk(posFile, JsonConvert.SerializeObject(holding13));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
               
            }
            
        }

        public string DownloadLatestPosition(string uri, string output)
        {
           
            var doc = _httpDataAgent.DownloadXml(uri);
           
            var json = _util.ToJson(_util.GetSpecialXmlElements("holding", new []{"infoTable"}, doc));

            return json;
          
        }

        public void Stop()
        { 
        }

        public string Name => "Edgar Service";
    }
}
