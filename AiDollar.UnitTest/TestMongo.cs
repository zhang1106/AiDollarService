using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiDollar.Edgar.Service;
using AiDollar.Infrastructure.Database;
using MongoDB.Driver;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AiDollar.UnitTest
{
    public class TestMongo
    {
        [Test]
        public void TestDatabaseCreate()
        {
            var db = new MongoDbOperation("mongodb://localhost:27017", "AiDollar");

            var util = new Util();
            var html = new HttpDataAgent();
            var doc = html.DownloadXml("https://www.sec.gov/Archives/edgar/data/1067983/000095012317007953/form13fInfoTable.xml");
            var json = util.ToJson(util.GetSpecialXmlElements("holding", new[] { "infoTable" }, doc));
            var holding = JsonConvert.DeserializeObject<HoldingRoot>(json);

            db.SaveItems(new[]{holding}, "Portfolio");
        }

        [Test]
        public void TestRead()
        {
            var db = new MongoDbOperation("mongodb://localhost:27017", "AiDollar");
            var port = db.Database.GetCollection<Portfolio>("Portfolio");
            var ports = port.Find("{'Cik':'0001067983'}").ToList();

            var dPorts = db.Select<Portfolio>("{'Cik':'0001067983'}");
        }

        [Test]
        public void TestSecurityRead()
        {
            var db = new MongoDbOperation("mongodb://localhost:27017", "AiDollar");
            var c = db.Database.GetCollection<Security>("Security");
             
            var ports = db.Select<Security>("{'Ticker':{$ne:'*'}}").ToList();

        }

        [Test]
        public void TestCusip()
        {
            var s = new Security()
            {
                Isin = "US01023E1001"
            };
            Assert.IsTrue(s.GetCusip()== "01023E100");
        }
    }
}
