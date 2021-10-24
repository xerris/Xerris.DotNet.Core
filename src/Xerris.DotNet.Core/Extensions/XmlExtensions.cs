using System.IO;
using System.Xml.Serialization;

namespace Xerris.DotNet.Core.Extensions
{
    public static class XmlExtensions
    {
        public static string ToXml<T>(this T subject)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, subject);
            return stringWriter.ToString();
        }

        public static T FromXml<T>(this string xml)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using var stringReader = new StringReader(xml);
            return (T)xmlSerializer.Deserialize(stringReader);
        }
    }
}