using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Upgrade.ProgramRelease
{
    public class Xml<T> where T : class, new()
    {
        private static string configPath = AppDomain.CurrentDomain.BaseDirectory + typeof(T).Name + ".xml";
        /// <summary>
        /// 保存为XML
        /// </summary>
        public void Save()
        {
            using (FileStream fileStream = new FileStream(configPath, FileMode.Create, FileAccess.Write))
            {
                new XmlSerializer(typeof(T)).Serialize(fileStream, this);
            }
        }
        /// <summary> 加载XML文件
        /// </summary>
        /// <typeparam name="Config"></typeparam>
        /// <param name="createInstanceWhenNonExists">如果文件不存在，指定一个创建函数</param>
        /// <returns>返回对象</returns>
        public static T Load()
        {
            if (!File.Exists(configPath))
            {
                return new T();
            }
            using (StreamReader streamReader = new StreamReader(configPath))
            {
                try
                {
                    return new XmlSerializer(typeof(T)).Deserialize(streamReader) as T;
                }
                catch (Exception ex)
                {
                    return new T();
                }
            }
        }
    }
}
