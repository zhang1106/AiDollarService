using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiDollar.Infrastructure.Hosting;
using Newtonsoft.Json;

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

        public EdgarService(IHttpDataAgent httpDataAgent, IUtil util, string[] ciks, string edgarUri, string posPage, 
            string outputPath)
        {
            _httpDataAgent = httpDataAgent;
            _util = util;
            _edgarUri = edgarUri;
            _ciks = ciks;
            _outputPath = outputPath;
            _posPage = posPage;
        }
        public void Start()
        {
            foreach (var cik in _ciks)
            {
                try
                {
                    var uri = string.Format(_edgarUri, cik);
                    var doc = _httpDataAgent.DownloadXml(uri);

                    var json = _util.ToJson(_util.GetSpecialXmlElements("root", "entry", doc));

                    var root = JsonConvert.DeserializeObject<EdgarEntry>(json);
                    var entry = root.root.entry.OrderByDescending(e => e.updated).FirstOrDefault();
                    var posIdxPage = entry.content.accession_nunber + "-index.htm";
                    var link = entry.link.href.Replace(posIdxPage, _posPage);
                    var posFile = _outputPath + "holding-" + cik + "-" + entry.content.accession_nunber + ".json";
                    DownloadLatestPosition(link, posFile);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
               
            }
            
        }

        public void DownloadLatestPosition(string uri, string output)
        {
           
            var doc = _httpDataAgent.DownloadXml(uri);
           
            var json = _util.ToJson(_util.GetSpecialXmlElements("holding", "infoTable", doc));
            _util.WriteToDisk(output,json);
        }

        public void Stop()
        { 
        }

        public string Name => "Edgar Service";
    }
}
