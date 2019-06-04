using System.IO;
using Transfe.Http.File.Tools;

namespace Transfe.Http.File.Entities
{
    /// <summary> 分块文件信息
    /// </summary>
    public class PartialFileInfo
    {
        private string md5;
        private FileInfo fileInfo;
        public long From { get; set; }
        public long To { get; set; }
        public bool IsPartial { get; set; }
        public long Length { get; set; }
        public long FileLength => this.fileInfo.Length;
        public string Name => this.fileInfo.Name;
        public string FilePath => this.fileInfo.FullName;
        public string MD5 => this.md5 ?? (this.md5 = MD5Tools.GetFileMd5(this.FilePath));
        public PartialFileInfo(string filePath)
        {
            this.fileInfo = new FileInfo(filePath);
            this.From = 0;
            this.To = this.FileLength - 1;
            this.IsPartial = false;
            this.Length = this.FileLength;
        }
    }
}
