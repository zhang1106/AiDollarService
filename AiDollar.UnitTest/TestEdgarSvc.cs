using System;
using System.Linq;
using System.Xml.Linq;
using AiDollar.Edgar.Service;
using AiDollar.Edgar.Service.Model;
using AiDollar.Edgar.Service.Service;
using AiDollar.Infrastructure.Logger;
using NUnit.Framework;
using Moq;
using Newtonsoft.Json;

namespace AiDollar.UnitTest
{
    [TestFixture]
    public class TestEdgarSvc
    {
        [Test]
        public void TestJsonConversion()
        {
            const string xml = "<test>data</test>";
            var util = new Util();
            var json = util.ToJson(xml);
            Assert.IsTrue(json=="{\"test\":\"data\"}");
        }
      

        [Test]
        public void TestXDocQuery()
        {
            var uri =
                "https://www.sec.gov/cgi-bin/browse-edgar?action=getcompany&amp;CIK=0001067983&amp;type=13F-HR&amp;count=3&amp;output=atom";
            var doc = XDocument.Load(uri);
            var ns = doc.Root.Name.Namespace;
            var element = doc.Root;
            var elements = doc.Root.Elements(ns+"entry");

            var rootNode = new XElement("feed");
            var xdoc = new XDocument(rootNode);
            xdoc.Root.Add(elements);

            var des = xdoc.Descendants();
            var util = new Util();
            var json = util.ToJson(xdoc);
        }

        [Test]
        public void TestIdxParser()
        {
            var uri = "https://www.sec.gov/Archives/edgar/data/1072627/000114420417054408/";
            var httpAgent = new HttpDataAgent();
        

        }

       

        [Test]
        public void TestXmlConvert()
        {
            var orig = "https://www.sec.gov/Archives/edgar/data/1084869/0001084869-17-000018-index.html";
            var dest = "https://www.sec.gov/Archives/edgar/data/1084869/000108486917000018/primary_doc.xml";
            //var parser = new EdgarIdxSvc(null,null,null,9);
            //Assert.IsTrue(parser.GetXmlUri(orig)==dest);
        }

        [Test]
        public void TestF4Json()
        {
            var xmlUri = "https://www.sec.gov/Archives/edgar/data/1084869/000108486917000018/primary_doc.xml";
            var uri = "https://www.sec.gov/Archives/edgar/full-index/crawler.idx";
            var output = "c:/temp/f4.json";
            var httpAgent = new HttpDataAgent();
            var util = new Util();
            util.Logger = new Mock<ILogger>().Object;
            //var parser = new EdgarIdxSvc(httpAgent, util, uri, 9);
            //var json = parser.GetReportJson(xmlUri);
            //var acts = JsonConvert.DeserializeObject<F4Activity>(json);
        }
    }
}
