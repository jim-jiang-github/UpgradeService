using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Upgrade.Json
{
    public class FileDetail
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 所在版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 文件MD5
        /// </summary>
        public string MD5 { get; set; }
        /// <summary>
        /// 文件MD5
        /// </summary>
        public long Length { get; set; }
        public FileDetail() { }
    }
}
