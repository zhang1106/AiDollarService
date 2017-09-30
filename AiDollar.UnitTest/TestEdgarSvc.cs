using System;
using System.Runtime.Remoting;
using System.Xml.Linq;
using AiDollar.Edgar.Service;
using NUnit.Framework;

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
        public void TestXmlQuery()
        {
            const string xml = @"<tests><test><t1>data</t1></test><test><t1>data</t1></test><test1>data</test1></tests>";
            var util = new Util();
            var sXml = util.GetSpecialXmlElements("root", "test", xml);
            Assert.IsTrue(sXml == "<root><test><t1>data</t1></test><test><t1>data</t1></test></root>");

            //var json = util.ToJson(sXml);
            //Assert.IsTrue(json == "{\"root\":{\"test\":[\"data\",\"data\"]}}");
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
    }
}
