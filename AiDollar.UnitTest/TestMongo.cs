﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiDollar.Edgar.Service;
using AiDollar.Infrastructure.Database;
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

    }
}
