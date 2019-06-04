using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Upgrade.Program.Client
{
    internal class JsonConvert
    {
        internal static T DeserializeObject<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
                return (T)dataContractJsonSerializer.ReadObject(memoryStream);
            }
        }
        internal static string SerializeObject(object value)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(value.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, value);
                byte[] dataBytes = new byte[memoryStream.Length];
                memoryStream.Position = 0;
                memoryStream.Read(dataBytes, 0, (int)memoryStream.Length);
                return Encoding.UTF8.GetString(dataBytes);
            }
        }
    }
}
