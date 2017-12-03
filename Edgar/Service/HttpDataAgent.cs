using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AiDollar.Edgar.Service
{
    public class HttpDataAgent : IHttpDataAgent
    {
        public async Task<byte[]> DownloadPageAsync(string page)
        {
            // ... Use HttpClient.
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(page))
            using (var content = response.Content)
            {
                // ... Read the string.
                var result = await content.ReadAsByteArrayAsync();

                return result;
            }
        }

        public XDocument DownloadXml(string page)
        {
            return XDocument.Load(page);
        }

        public string Download(string page)
        {
            using (var client = new WebClient())
            {
                var content = client.DownloadString(page);
                return content;
            }
        }

    }

    public static class DocumentExtensions
    {
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
    }
}