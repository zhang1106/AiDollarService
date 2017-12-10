﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AiDollar.Edgar.Service.Model;
using AiDollar.Infrastructure.Database;
using AiDollar.Infrastructure.Logger;
using Newtonsoft.Json;

namespace AiDollar.Edgar.Service.Service
{
    public class F4Svc : IF4Svc
    {
        private readonly IEdgarIdxSvc _edgarIdxSvc;
        private readonly IHttpDataAgent _httpDataAgent;
        private readonly IUtil _util;
        private readonly IAiDbSvc _aiDbSvc;
        private readonly IDbOperation _dbOperation;
        
        public ILogger Logger { get; set; }

        public F4Svc(IEdgarIdxSvc edgarIdxSvc, IHttpDataAgent httpDataAgent, IUtil util, IAiDbSvc aiDbSvc, IDbOperation dbOperation)
        {
            _edgarIdxSvc = edgarIdxSvc;
            _httpDataAgent = httpDataAgent;
            _util = util;
            _aiDbSvc = aiDbSvc;
            _dbOperation = dbOperation;
        }

        public IEnumerable<InsideTrade> GetLatestInsideTrades(int days)
        {
            var trades = _aiDbSvc.GetInsideTrades();
            var selected = trades.Where(t=>(DateTime.Now.Subtract(DateTime.Parse(t.TransactionDate)).Days<days) && 
            (decimal.Parse(t.Amount) > 10000 && decimal.Parse(t.Amount)/ (decimal.Parse(t.Amount)+decimal.Parse(t.RemainAmount))>0.1m));
            return selected;
        }

        public IEnumerable<InsideTrade> DownloadLatestInsideTrades(int days)
        {
            var trades = DownloadLatestInsideTradesFromEdgar(days);
            //save to db
            var saved = _aiDbSvc.GetInsideTrades().ToLookup(t=>new {t.Issuer, t.Reporter,t.TransactionDate});
            var toSave = trades.Where(t => !saved.Contains(new {t.Issuer, t.Reporter, t.TransactionDate}));
           
            _dbOperation.SaveItems(toSave, "InsideTrade");
            return toSave.ToList();
        }

        public IEnumerable<InsideTrade> DownloadLatestInsideTradesFromEdgar(int days)
        {
            var latestF4 = GetLatestF4Idx(days);
            var f4Activities = new List<F4Activity>();
            foreach (var idx in latestF4)
            {
                var f4 = GetF4Report(idx.URL);
                Logger.LogInformation($"F4xml: Get {idx.URL}");
                Console.WriteLine($"F4xml: Get {idx.URL}");

                if (f4 == null) continue;

                var trans = f4.ownershipDocument?.nonDerivativeTable?.GetNonDerivativeTransaction();
                Console.WriteLine($"transact code:{trans?.transactionCoding}");
                if(trans?.transactionCoding.transactionCode=="S" || trans?.transactionCoding.transactionCode == "P")
                    f4Activities.Add(f4);
            }
            
            var latest = f4Activities.Select(GetInsideTrade);
            return latest;
        }

        private IEnumerable<EdgarIdx> GetLatestF4Idx(int days)
        {
            var desc = _edgarIdxSvc.DownLoadEdgarIdx();
            var idxes = _edgarIdxSvc.Parse(desc);
            var f4s = idxes
                .Where(e => e.FormType == "4" && DateTime.Now.Subtract(e.DateFiled).Days < days);
            return f4s;
        }

        private F4Activity GetF4Report(string idxUrl)
        {
            try
            {
                var xmlUrl = GetXmlUri(idxUrl);
                var json = GetF4String(xmlUrl);
                if (json != null)
                {
                    return JsonConvert.DeserializeObject<F4Activity>(json);
                }

                var xmlFile = GetXmlFiles(GetXmlFolder(idxUrl));
                if (!xmlFile.Any()) return null;

                xmlUrl = "https://www.sec.gov/" + xmlFile[0];
                json = GetF4String(xmlUrl);

                return json != null ? JsonConvert.DeserializeObject<F4Activity>(json) : null;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                return null;
            }
           
        }

        private string GetF4String(string uri)
        {
            try
            {
                var xml = _httpDataAgent.DownloadXml(uri);
                var json = _util.ToJson(xml);
                return json;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private IList<string> GetXmlFiles(string url)
        {
            var content = _httpDataAgent.Download(url);
            var regex = new Regex("Archives/edgar[^\"]*[.]xml");

            var matches = regex.Matches(content);
            var list = new List<string>();
            if (matches.Count <= 0) return list;

            list.AddRange(from Match match in matches where match.Success select match.Value);

            return list;
        }


        private string GetXmlFolder(string URL)
        {
            var nodash = URL.Replace("-", string.Empty);
            var xml = nodash.Replace("index.htm", "/");
            return xml;
        }

        private string GetXmlUri(string URL)
        {
            var nodash = URL.Replace("-", string.Empty);
            var xml = nodash.Replace("index.htm", "/primary_doc.xml");
            
            return xml;
        }

        private InsideTrade GetInsideTrade(F4Activity activity)
        {
            var iTrade = new InsideTrade
            {
                Amount = activity.ownershipDocument.nonDerivativeTable.GetNonDerivativeTransaction().transactionAmounts
                    .transactionShares.value,
                Price = activity.ownershipDocument.nonDerivativeTable.GetNonDerivativeTransaction().transactionAmounts
                    .transactionPricePerShare.value,
                Cik = activity.ownershipDocument.issuer.issuerCik,
                Issuer = activity.ownershipDocument.issuer.issuerName,
                Symbol = activity.ownershipDocument.issuer.issuerTradingSymbol,
                RemainAmount = activity.ownershipDocument.nonDerivativeTable.GetNonDerivativeTransaction()
                    .postTransactionAmounts.sharesOwnedFollowingTransaction.value,
                Reporter = activity.ownershipDocument.reportingOwner.reportingOwnerId.rptOwnerName,
                Role = activity.ownershipDocument.reportingOwner.reportingOwnerRelationship.officerTitle,
                TransactionCode = activity.ownershipDocument.nonDerivativeTable.GetNonDerivativeTransaction()
                    .transactionCoding.transactionCode=="S"?"Sale":"Buy",
                TransactionDate = activity.ownershipDocument.nonDerivativeTable.GetNonDerivativeTransaction().transactionDate
                    .value,
                _id = Guid.NewGuid()
            };
            return iTrade;
        }
    }
}