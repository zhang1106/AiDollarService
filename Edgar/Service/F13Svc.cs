using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using AiDollar.Edgar.Model;
using AiDollar.Edgar.Service.Model;
using AiDollar.Infrastructure.Database;
using Newtonsoft.Json;

namespace AiDollar.Edgar.Service.Service
{
    public class F13Svc : IF13Svc
    {
        private readonly IAiPortfolioSvc _aiPortfolioSvc;
        private readonly IDbOperation _dbOperation;
        private readonly IAiDbSvc _aiDbSvc;
        private readonly string _edgarUri;
        private readonly IHttpDataAgent _httpDataAgent;
       
        private readonly string _posPage;
        private readonly IUtil _util;

        public F13Svc(IHttpDataAgent httpDataAgent, IUtil util, IAiDbSvc aiDbSvc, IAiPortfolioSvc aiPortfolioSvc,
            IDbOperation dbOperation, string edgarUri, string posPage)
        {
            _httpDataAgent = httpDataAgent;
            _util = util;
            _edgarUri = edgarUri;
            _aiPortfolioSvc = aiPortfolioSvc;
            
            _posPage = posPage;
            _dbOperation = dbOperation;
            _aiDbSvc = aiDbSvc;
        }

        public void GetLatest13F(string outputPath, bool downloadNew)
        {
            if (downloadNew)
            {
                GetLatest13F(outputPath);
                return;
            }
            OutputAllPortfolios(outputPath);
        }
        public void GetLatest13F(string outputPath)
        {

            var gurus = _aiDbSvc.GetGurus();

            foreach (var guru in gurus)
            {
                Console.WriteLine($"Getting Data for {guru.Fund}");
                try
                {
                    var cik = guru.Cik;

                    var uri = string.Format(_edgarUri, cik);
                    var doc = _httpDataAgent.DownloadXml(uri);

                    var json = _util.ToJson(
                        _util.GetSpecialXmlElements("root", new[] {"company-info", "entry"}, doc));

                    var root = JsonConvert.DeserializeObject<RootObject>(json);

                    if (root.root.entry == null) continue;

                    var entries = root.root.entry.Where(e => e.updated > DateTime.Now.AddMonths(-18));

                    foreach (var entry in entries)
                    {
                        var posIdxPage = entry.content.accession_nunber + "-index.htm";
                        var link = entry.link.href.Replace(posIdxPage, _posPage);

                        var jPos = TryDownloadLatestPosition(link);

                        if (jPos == null) continue;

                        var holding = JsonConvert.DeserializeObject<HoldingRoot>(jPos);
                        var holding13 = new Portfolio
                        {
                            infoTable = holding.holding.infoTable,
                            Cik = cik,
                            ReportedDate = entry.updated,
                            Holder = root.root.company_info.conformed_name,
                            _id = entry.content.accession_nunber
                        };

                        if (!ReportExists(holding13))
                            _dbOperation.SaveItems(new[] {holding13}, "Portfolio");
                    }
                    Console.WriteLine($"Done for {guru.Fund}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Cant get data for {guru.Fund}");
                }
            }

            Console.WriteLine($"Start recal portfolio to file");

            OutputAllPortfolios(outputPath);
        }

        private void OutputAllPortfolios(string outputPath)
        {
            var gurus = _aiDbSvc.GetGurus();
            var securities = _aiDbSvc.GetSecurities().ToLookup(s => s.Cusip);
            var ports = new List<AiPortfolio>();
            var guruWithData = new List<Guru>();
            foreach (var g in gurus)
            {
                Console.Write($"Calc for {g.Cik}");
                var p = _aiPortfolioSvc.GetPortfolio(g.Cik);
                if (p == null) continue;
                p.Fund = p.Owner;
                p.Owner = g.Owner;
                ports.Add(p);
                guruWithData.Add(g);
            }
            var pbyCik = ports.ToDictionary(p => p.Cik);

            //security
            var holdings = ports.SelectMany(p => p.Holdings.Select(Port => new {p.Owner, p.Cik, Port}));

            var group = holdings.GroupBy(h => h.Port.Cusip);

            var sHoldings = group.Select(g => new AiSecurityHolding
            {
                Cusip = g.Key,
                Holding = g.Select(a => new AiSecurityHoldingUnit
                    {
                        Owner = a.Owner,
                        Cik = a.Cik,
                        Recent = a.Port.Share4,
                        Q1 = a.Port.Share3,
                        Q2 = a.Port.Share2,
                        Q3 = a.Port.Share1,
                        Q4 = a.Port.Share0
                    }
                ).OrderByDescending(h => h.Recent).ToList()
            }).OrderByDescending(s => s.Holding.Count);

            var hashSHoldings = sHoldings.ToDictionary(s => s.Cusip);
            var top25 = sHoldings.Take(50);

            var security = top25.Where(t => securities[t.Cusip].FirstOrDefault()?.SecurityDesc != null)
                .Select(h => new
                {
                    Issuer = securities[h.Cusip].FirstOrDefault()?.SecurityDesc,
                    h.Cusip,
                    securities[h.Cusip].FirstOrDefault()?.Ticker
                });

            var tickerToCusip = new Dictionary<string, string>();
            foreach (var h in holdings.Where(s => !string.IsNullOrEmpty(s.Port.Ticker)))
            {
                if (tickerToCusip.ContainsKey(h.Port.Ticker))
                {
                    tickerToCusip.Add(h.Port.Ticker, h.Port.Cusip);
                }
            } 

            _util.WriteToDisk(outputPath + "holdByCik.json", JsonConvert.SerializeObject(pbyCik));
            _util.WriteToDisk(outputPath + "guru.json",
                JsonConvert.SerializeObject(guruWithData.OrderBy(g => g.Rank)
                    .Select(g => new {g.Cik, g.Owner, g.Fund})));
            _util.WriteToDisk(outputPath + "holdByCusip.json", JsonConvert.SerializeObject(hashSHoldings));
            _util.WriteToDisk(outputPath + "security.json", JsonConvert.SerializeObject(security));
            _util.WriteToDisk(outputPath + "tickerToCusip.json", JsonConvert.SerializeObject(tickerToCusip));
        }

        private bool ReportExists(Portfolio portfolio)
        {
            var query = $"{{'_id':'{portfolio._id}'}}";
            var portfolios = _dbOperation.Select<Portfolio>(query);
            return portfolios.Any();
        }

        public string TryDownloadLatestPosition(string uri)
        {
            try
            {
                return DownloadLatestPosition(uri);
            }
            catch (Exception e)
            {
                Console.WriteLine("cant downlpad " + uri + " try infotable.xml.");
                var tryUri = uri.Replace(_posPage, "infotable.xml");
                return DownloadLatestPosition(tryUri);
            }
        }

        public string DownloadLatestPosition(string uri)
        {
            var doc = _httpDataAgent.DownloadXml(uri);

            var json = _util.ToJson(_util.GetSpecialXmlElements("holding", new[] {"infoTable"}, doc));

            return json;
        }
    }
}