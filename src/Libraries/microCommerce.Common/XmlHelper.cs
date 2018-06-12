using System.IO;
using System.Xml.Serialization;

namespace microCommerce.Common
{
    public class XmlHelper
    {
        public static void Serialize<T>(string filePath, T item)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = File.Open(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, item);
            }
        }

        public static T Deserialize<T>(string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = File.OpenRead(filePath))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}