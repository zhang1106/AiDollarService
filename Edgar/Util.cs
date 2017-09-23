using System.Xml;
using Newtonsoft.Json;

namespace AiDollar.Edgar.Service
{
    public class Util:IUtil
    {
        public string ToJson(byte[] xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(System.Text.Encoding.Default.GetString(xml));
            var json = JsonConvert.SerializeXmlNode(doc);
            return json;
        }
    }
}
