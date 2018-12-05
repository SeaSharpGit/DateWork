using System;
using System.IO;
using System.Xml.Serialization;

namespace DateWork.Helpers
{
    public static class XmlHelper
    {
        /// <summary>
        /// XML反序列化为实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T XmlToObject<T>(this string fileName)
        {
            Object obj = null;
            var xs = new XmlSerializer(typeof(T));
            using (var stream = File.OpenRead(fileName))
            {
                obj = xs.Deserialize(stream);
            }
            return (T)obj;
        }
        
        /// <summary>
        /// 实体类序列化为XML
        /// </summary>
        /// <param name="xobj"></param>
        /// <param name="filename"></param>
        public static void ObjectToXml(this object xobj, string filename)
        {
            int index = filename.LastIndexOf('\\');
            if (index > 0)
            {
                string path = filename.Substring(0, index);
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
            }
            XmlSerializer xs = new XmlSerializer(xobj.GetType());
            using (var stream = System.IO.File.Open(filename, FileMode.Create, FileAccess.Write))
            {
                xs.Serialize(stream, xobj);
            }
        }

    }
}