using Share.Transfe.Tools;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Share.Transfe.File.Entities
{
    public class DownloadFile : IDisposable
    {
        public static string Ext { get; } = ".downloadPart";
        /// <summary>
        /// 是否是本地加载的还是从url加载的
        /// </summary>
        private bool isLocal = true;
        private bool isAppend = false;
        private string downloadPartPath = string.Empty;
        private FileStream fileStream = null;
        public string URL { get; set; }
        public string MD5 { get; set; }
        public long Length { get; set; }
        public long RangeBegin => (this.fileStream?.Length ?? 0) - Position;
        public int Position => (this.isLocal || this.isAppend) ? (Encoding.UTF8.GetBytes(this.URL).Length + Encoding.UTF8.GetBytes(this.MD5 ?? string.Empty).Length + 4 + 4) : 0;
        private DownloadFile()
        {
        }
        /// <summary>
        /// 从url加载对象
        /// </summary>
        /// <param name="downloadPartPath">文件路径</param>
        /// <param name="uRL">url</param>
        /// <param name="mD5">文件md5</param>
        /// <returns></returns>
        public static DownloadFile FromUrl(string downloadPartPath, string uRL)
        {
            DownloadFile downloadFile = new DownloadFile();
            downloadFile.downloadPartPath = downloadPartPath;
            downloadFile.URL = uRL;
            //downloadFile.fileStream = System.IO.File.Create(downloadPartPath);
            downloadFile.isLocal = false;
            return downloadFile;
        }
        /// <summary>
        /// 执行本地化，如果是url下载 则赋值url上的md5，如果是本地文件续传则对比md5，md5不一致返回false并且删除本地文件
        /// </summary>
        /// <param name="length"></param>
        /// <param name="mD5"></param>
        /// <returns></returns>
        public bool DoLocal(long? length, string mD5)
        {
            this.Length = length ?? -1;
            if (this.isLocal)
            {
                if (this.MD5 != mD5)
                {
                    this.fileStream.Dispose();
                    System.IO.File.Delete(this.downloadPartPath);
                    return false;
                }
            }
            else
            {
                this.fileStream = System.IO.File.Create(this.downloadPartPath);
                this.MD5 = MD5;
                this.AppendFileStream();
            }
            return true;
        }
        /// <summary> 从文件流加载对象
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static DownloadFile FromPartPath(string downloadPartPath)
        {
            DownloadFile downloadFile = new DownloadFile();
            downloadFile.downloadPartPath = downloadPartPath;
            if (System.IO.File.Exists(downloadPartPath))
            {
                //downloadFile.fileInfo = new FileInfo(partPath);
                downloadFile.fileStream = System.IO.File.Open(downloadPartPath, FileMode.Open, FileAccess.ReadWrite);
                if (downloadFile.fileStream.Length == 0)
                {
                    downloadFile.fileStream.Dispose();
                    System.IO.File.Delete(downloadPartPath);
                    return null;
                }
                byte[] urlLengthBytes = new byte[4];
                downloadFile.fileStream.Read(urlLengthBytes, 0, 4);
                int urlLength = BitConverter.ToInt32(urlLengthBytes, 0);
                byte[] urlBytes = new byte[urlLength];
                downloadFile.fileStream.Read(urlBytes, 0, urlLength);

                byte[] md5LengthBytes = new byte[4];
                downloadFile.fileStream.Read(md5LengthBytes, 0, 4);
                int md5Length = BitConverter.ToInt32(md5LengthBytes, 0);
                byte[] md5Bytes = new byte[md5Length];
                downloadFile.fileStream.Read(md5Bytes, 0, md5Length);

                //byte[] lengthBytes = new byte[8];
                //downloadFile.fileStream.Read(lengthBytes, 0, 8);

                downloadFile.URL = Encoding.UTF8.GetString(urlBytes);
                downloadFile.MD5 = Encoding.UTF8.GetString(md5Bytes);
                downloadFile.MD5 = downloadFile.MD5 == string.Empty ? null : downloadFile.MD5;
                //downloadFile.Length = BitConverter.ToInt64(lengthBytes, 0);

                downloadFile.fileStream.Position = downloadFile.fileStream.Length;
            }
            return downloadFile;
        }
        public async Task Write(byte[] array, int offset, int count)
        {
            this.fileStream.Write(array, offset, count);
            await this.fileStream.FlushAsync();
        }
        /// <summary> 吧对象追加到文件流的结尾
        /// </summary>
        private void AppendFileStream()
        {
            byte[] urlBytes = Encoding.UTF8.GetBytes(this.URL);
            int urlLength = urlBytes.Length;
            byte[] urlLengthBytes = BitConverter.GetBytes(urlLength);
            this.fileStream.Write(urlLengthBytes, 0, urlLengthBytes.Length);
            this.fileStream.Write(urlBytes, 0, urlBytes.Length);

            byte[] md5Bytes = Encoding.UTF8.GetBytes(this.MD5 ?? string.Empty);
            int md5Length = md5Bytes.Length;
            byte[] md5LengthBytes = BitConverter.GetBytes(md5Length);
            this.fileStream.Write(md5LengthBytes, 0, md5LengthBytes.Length);
            this.fileStream.Write(md5Bytes, 0, md5Bytes.Length);
            this.fileStream.Flush();
            //byte[] lengthBytes = BitConverter.GetBytes(this.Length);
            //this.fileStream.Write(lengthBytes, 0, lengthBytes.Length);
            this.isAppend = true;
        }

        public void Dispose()
        {
            this.fileStream?.Dispose();
        }
        /// <summary>
        /// 将分块续传的文件释放出来并删除分块续传文件
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Release()
        {
            string filePath = this.downloadPartPath.Replace(DownloadFile.Ext, string.Empty);
            using (FileStream write = System.IO.File.Create(filePath))
            {
                this.fileStream.Position = this.Position;
                await this.fileStream.CopyToAsync(write);
            }
            //校验服务器上的md5 如果服务器上下载数据不包含md5则视为md5校验通过
            if (this.MD5 == null || this.MD5 == MD5Tools.GetFileMd5(filePath))
            {
                this.fileStream?.Dispose();
                System.IO.File.Delete(this.downloadPartPath);
                return true;
            }
            else
            {
                System.IO.File.Delete(filePath);
                return false;
            }
        }
    }
}
