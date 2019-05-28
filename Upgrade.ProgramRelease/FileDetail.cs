using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Upgrade.ProgramRelease
{
    public class FileScan
    {
        public enum ScanResult
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 0,
            /// <summary>
            /// 新增
            /// </summary>
            Add = 1,
            /// <summary>
            /// 移除
            /// </summary>
            Remove,
            /// <summary>
            /// 更新
            /// </summary>
            Update
        }
        public string Name { get; set; }
        public string MD5 { get; set; }
        public long Length { get; set; }
        public string Version { get; set; }
        public ScanResult Result { get; set; }
        public FileScan() { }
        public FileScan(string path)
        {
            this.Length = new FileInfo(path).Length;
            this.Version = System.Diagnostics.FileVersionInfo.GetVersionInfo(path).FileVersion;
            using (var stream = File.Open(path, FileMode.Open))
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                this.MD5 = string.Concat(md5.ComputeHash(stream).Select(b => b.ToString("x2"))).ToUpper();
                this.Name = Path.GetFileName(path);
            }
        }
        public override string ToString()
        {
            return Name + "-----" + MD5;
        }
    }
}
