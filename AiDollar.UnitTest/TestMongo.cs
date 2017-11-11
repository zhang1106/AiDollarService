using System.Linq;
using AiDollar.Edgar.Model;
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
             
            var securities = db.Select<Security>("{'Ticker':'Ticker'}").ToList();

        }

        
    }
}
