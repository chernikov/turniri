using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace turniri.Tools.Qiwi
{
    /// <summary>
    /// Utility methods
    /// </summary>
    public static class Utilities
    {
        public static string XMLSerialize<T>(T data)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(String.Empty, String.Empty);

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);

            serializer.Serialize(sw, data, ns);

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static T XMLDeserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new StringReader(xml));
        }
    }
}