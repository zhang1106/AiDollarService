using System.Threading.Tasks;
using System.Xml.Linq;

namespace AiDollar.Edgar.Service
{
    public interface IHttpDataAgent
    {
         Task<byte[]> DownloadPageAsync(string page);

         XDocument DownloadXml(string page);
    }
}
