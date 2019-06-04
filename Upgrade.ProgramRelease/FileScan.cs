using Share.Upgrade.Json;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Upgrade.ProgramRelease
{
    public class FileScan
    {
        /// <summary> 比较结果
        /// </summary>
        public enum CompareResult
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 0,
            /// <summary>
            /// 新增
            /// </summary>
            Filter = 1,
            /// <summary>
            /// 新增
            /// </summary>
            Add = 2,
            /// <summary>
            /// 移除
            /// </summary>
            Remove = 3,
            /// <summary>
            /// 更新
            /// </summary>
            Update = 4
        }
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// 文件MD5
        /// </summary>
        public string MD5 { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// 文件版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 文件比对结果
        /// </summary>
        public CompareResult Result { get; set; }
        public FileScan(JsonFileDetail jsonFileDetail)
        {
            this.Name = jsonFileDetail.Name;
            this.MD5 = jsonFileDetail.MD5;
            this.Length = jsonFileDetail.Length;
        }
        public FileScan(string dir, string name, bool calcMD5 = true)
        {
            this.FullPath = Path.Combine(dir, name);
            this.Length = new FileInfo(this.FullPath).Length;
            this.Version = System.Diagnostics.FileVersionInfo.GetVersionInfo(this.FullPath).FileVersion;
            this.Name = name;
            if (calcMD5)
            {
                using (var stream = File.Open(this.FullPath, FileMode.Open))
                using (MD5 md5 = new MD5CryptoServiceProvider())
                {
                    this.MD5 = string.Concat(md5.ComputeHash(stream).Select(b => b.ToString("x2"))).ToUpper();
                };
            }
        }
        public override string ToString()
        {
            return Name + "-----" + MD5;
        }
    }
}
