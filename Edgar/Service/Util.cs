using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AiDollar.Infrastructure.Logger;
using Newtonsoft.Json;

namespace AiDollar.Edgar.Service
{
    public class Util : IUtil
    {
        public ILogger Logger { get; set; }

        public string ToJson(XDocument doc)
        {
            var xmldoc = doc.ToXmlDocument();
            var builder = new StringBuilder();
            JsonSerializer.Create().Serialize(new CustomJsonWriter(new StringWriter(builder)), xmldoc);
            var serialized = builder.ToString();
            return serialized;
        }

        public string ToJson(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var json = JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        public XDocument GetSpecialXmlElements(string root, IEnumerable<string> names, XDocument xml)
        {
            var ns = xml.Root?.Name.Namespace;
            var rootElement = new XElement(root);
            var xdoc = new XDocument(rootElement);
            foreach (var tag in names)
            {
                var elements = xml.Descendants(ns + tag);
                xdoc.Root?.Add(elements);
            }

            var des = xdoc.Descendants();
            foreach (var d in des)
            {
                var newLoc = d.Name.LocalName.Replace("-", "_");
                d.Name = d.Name.Namespace + newLoc;
            }
            return xdoc;
        }


        public void WriteToDisk(string path, string data)
        {
            try
            {
                File.WriteAllText(path, data);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message + e.StackTrace);
            }
        }
    }

    public class CustomJsonWriter : JsonTextWriter
    {
        public CustomJsonWriter(TextWriter writer) : base(writer)
        {
        }

        public override void WritePropertyName(string name)
        {
            if (name.StartsWith("@") || name.StartsWith("#"))
                base.WritePropertyName(name.Substring(1));
            else
                base.WritePropertyName(name);
        }
    }
}