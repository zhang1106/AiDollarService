using System.Collections.Generic;
using System.Xml.Linq;

namespace AiDollar.Edgar.Service
{
    public interface IUtil
    {
        string ToJson(string xml);
        string ToJson(XDocument xml);
      
        XDocument GetSpecialXmlElements(string root, IEnumerable<string> tags, XDocument xml);
        void WriteToDisk(string path, string data);
    }
}
