using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace artfulplace.Ilisgate.V2
{
    internal class DataParser
    {
        internal static T CreateDataFromXml<T>(string content)
        {
            T data;
            using (var stream = new MemoryStream())
            {
                Byte[] buf = Encoding.UTF8.GetBytes(content);
                stream.Write(Encoding.UTF8.GetBytes(content), 0, buf.Length);
                stream.Seek(0, SeekOrigin.Begin);
                data = (T)(new DataContractSerializer(typeof(T))).ReadObject(stream);
            }
            return data;
        }

        internal static T CreateDataFromJson<T>(string content)
        {
            T data;
            using (var stream = new MemoryStream())
            {
                Byte[] buf = Encoding.UTF8.GetBytes(content);
                stream.Write(Encoding.UTF8.GetBytes(content), 0, buf.Length);
                stream.Seek(0, SeekOrigin.Begin);

                // BOM変換バグ回避で、3つずらす
                var pos = stream.ReadByte() == 0xef ? 3 : 0;
                stream.Seek(pos, SeekOrigin.Begin);

                data = (T)(new DataContractJsonSerializer(typeof(T))).ReadObject(stream);
            }
            return data;
        }

        internal static string WriteObjectToXml<T>(T data)
        {
            var mstream = new MemoryStream();
            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(mstream, data);
            mstream.Seek(0, SeekOrigin.Begin);
            using (var xr = new StreamReader(mstream))
            {
                return xr.ReadToEnd();
            }
        }

        internal static string WriteObjectToJson<T>(T data)
        {
            var mstream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(T));
            serializer.WriteObject(mstream, data);
            mstream.Seek(0, SeekOrigin.Begin);
            using (var xr = new StreamReader(mstream))
            {
                return xr.ReadToEnd();
            }
        }
    }
}
