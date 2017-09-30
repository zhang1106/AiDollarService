using System.Xml.Linq;

namespace AiDollar.Edgar.Service
{
    public interface IUtil
    {
        string ToJson(string xml);
        string ToJson(XDocument xml);
        string ConvertNReplace(byte[] ary);
        string GetSpecialXmlElements(string root, string tag, string xml);
        XDocument GetSpecialXmlElements(string root, string tag, XDocument xml);
        void WriteToDisk(string path, string data);
    }
}
