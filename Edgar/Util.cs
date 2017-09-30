using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AiDollar.Infrastructure.Logger;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace AiDollar.Edgar.Service
{
    public class Util:IUtil
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

        public string GetSpecialXmlElements(string root, string name, string xml)
        {
            var  doc = XDocument.Parse(xml);
            var elements = doc.Descendants(name);
            return  $"<{root}>{string.Join("", elements.Select(e=>e.ToString()))}</{root}>";
          
        }

        public XDocument GetSpecialXmlElements(string root, string name, XDocument xml)
        {
            var ns = xml.Root?.Name.Namespace;
            var elements = xml.Descendants(ns+name);
             
            var rootElement = new XElement(root);
            var xdoc = new XDocument(rootElement);
            xdoc.Root?.Add(elements);

            var des = xdoc.Descendants();
            foreach (var d in des)
            {
                var newLoc = d.Name.LocalName.Replace("-", "_");
                d.Name = d.Name.Namespace + newLoc;
            }
            return xdoc;
        }

        public string ConvertNReplace(byte[] ary)
        {
            var str = System.Text.Encoding.Default.GetString(ary);
            return str.Replace("@", "__");
        }

        public void WriteToDisk(string path, string data)
        {
            try
            {
                System.IO.File.WriteAllText(path,  data);
            }
            catch (Exception e)
            {
              Logger.LogError(e.Message + e.StackTrace);
            }
        }
    }

    public class CustomJsonWriter : JsonTextWriter
    {
        public CustomJsonWriter(TextWriter writer) : base(writer) { }

        public override void WritePropertyName(string name)
        {
            if (name.StartsWith("@") || name.StartsWith("#"))
            {
                base.WritePropertyName(name.Substring(1));
            }
            else
            {
                base.WritePropertyName(name);
            }
        }
    }
}
 