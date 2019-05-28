using FileTransfe.Entities;
using FileTransfe.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfe.Core
{
    /// <summary>
    /// 下载进度变化事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="progress">进度</param>
    public delegate void DownloadProgressChangedHandler(object sender, float? progress);
    /// <summary>
    /// 下载速度变化事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="speed">速度</param>
    public delegate void DownloadSpeedChangedHandler(object sender, float speed);
    /// <summary>
    /// 下载完成事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="result"></param>
    public delegate void DownloadCompletedHandler(object sender, bool done, string result);
    /// <summary>
    /// 下载错误事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="result"></param>
    public delegate void DownloadErrorHandler(object sender, string result);
    /// <summary> 客户端下载
    /// </summary>
    public class ClientDownload
    {
        #region 事件
        /// <summary>
        /// 下载进度变化事件
        /// </summary>
        public event DownloadProgressChangedHandler DownloadProgressChanged;
        protected virtual void OnDownloadProgressChanged(float? progress)
        {
            this.DownloadProgressChanged?.Invoke(this, progress);
        }
        /// <summary>
        /// 下载速度变化事件
        /// </summary>
        public event DownloadSpeedChangedHandler DownloadSpeedChanged;
        protected virtual void OnDownloadSpeedChanged(float speed)
        {
            this.DownloadSpeedChanged?.Invoke(this, speed);
        }
        /// <summary>
        /// 下载完成事件
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="speed"></param>
        public event DownloadCompletedHandler DownloadCompleted;
        protected virtual void OnDownloadCompleted(bool done, string result = null)
        {
            this.DownloadCompleted?.Invoke(this, done, result);
        }
        /// <summary>
        /// 下载错误事件
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="speed"></param>
        public event DownloadErrorHandler DownloadError;
        protected virtual void OnDownloadError(string result)
        {
            this.DownloadError?.Invoke(this, result);
        }
        #endregion
        #region 属性
        /// <summary> 下载的url
        /// </summary>
        private string DownloadUrl { get; set; }
        #endregion
        public ClientDownload(string downloadUrl)
        {
            this.DownloadUrl = downloadUrl;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async void ResumeDownload(string partPath)
        {
            bool success = false;
            using (FileStream fileStream = File.Open(partPath, FileMode.Open))
            using (HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) })
            {
                DownloadFile downloadFile = DownloadFile.FromFileStream(fileStream);
                if (downloadFile == null) //为空说明已经完成
                {
                    success = true;
                }
                else
                {
                    try
                    {
                        httpClient.DefaultRequestHeaders.Range = new RangeHeaderValue(downloadFile.RangeBegin, null);
                        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(downloadFile.URL, HttpCompletionOption.ResponseHeadersRead);
                        long? contentLength = httpResponseMessage.Content.Headers.ContentLength;
                        if (httpResponseMessage.Content.Headers.ContentRange != null) //如果为空，则说明服务器不支持断点续传
                        {
                            contentLength = httpResponseMessage.Content.Headers.ContentRange.Length;//服务器上的文件大小
                        }
                        string md5 = httpResponseMessage.Content.Headers.ContentMD5 == null ? null : Convert.ToBase64String(httpResponseMessage.Content.Headers.ContentMD5);
                        if (md5 != downloadFile.MD5)
                        {
                            throw new Exception("服务器的上的版本已经变化");
                        }
                        Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
                        stream.ReadTimeout = 10 * 1000;
                        await Download(stream, fileStream, downloadFile);
                        if (md5 == null || md5 == MD5Tools.GetFileMd5(partPath))
                        {
                            success = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        //请求错误
                        this.OnDownloadError(ex.Message);
                    }
                }
            }
            if (success)
            {
                try
                {
                    File.Move(partPath, partPath.Replace(".downloadPart", ""));
                }
                catch (Exception ex)
                {
                    this.OnDownloadError(ex.Message);
                }
            }
            this.OnDownloadCompleted(success);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async void Download(string filePath)
        {
            bool success = false;
            string downloadPartPath = Path.Combine(filePath + ".downloadPart");
            string url = this.DownloadUrl + "?fileName=" + Path.GetFileName(filePath);
            using (FileStream fileStream = File.Create(downloadPartPath))
            using (HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) })
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Range = new RangeHeaderValue(0, null);
                    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                    long? contentLength = httpResponseMessage.Content.Headers.ContentLength;
                    if (httpResponseMessage.Content.Headers.ContentRange != null) //如果为空，则说明服务器不支持断点续传
                    {
                        contentLength = httpResponseMessage.Content.Headers.ContentRange.Length;//服务器上的文件大小
                    }
                    string md5 = httpResponseMessage.Content.Headers.ContentMD5 == null ? null : Convert.ToBase64String(httpResponseMessage.Content.Headers.ContentMD5);
                    Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
                    stream.ReadTimeout = 10 * 1000;
                    DownloadFile downloadFile = new DownloadFile()
                    {
                        Length = contentLength,
                        MD5 = md5,
                        RangeBegin = 0,
                        URL = url
                    };
                    await Download(stream, fileStream, downloadFile);
                    if (md5 == null || md5 == MD5Tools.GetFileMd5(downloadPartPath))
                    {
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    this.OnDownloadError(ex.Message);
                }
            }
            if (success)
            {
                try
                {
                    File.Move(downloadPartPath, filePath);
                }
                catch (Exception ex)
                {
                    this.OnDownloadError(ex.Message);
                }
            }
            this.OnDownloadCompleted(success);
        }
        private async Task Download(Stream downloadStream, FileStream fileStream, DownloadFile downloadFile)
        {
            int bufferSize = 1024 * 1024 * 100; //100M缓存
            byte[] buffer = new byte[bufferSize];
            long position = downloadFile.RangeBegin;
            int readLength = 0;
            try
            {
                decimal downloadSpeed = 0;//下载速度
                var beginSecond = DateTime.Now.Second;//当前时间秒
                while ((readLength = await downloadStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    position += readLength;
                    downloadSpeed += readLength;
                    fileStream.Write(buffer, 0, readLength);
                    if (position != downloadFile.Length)
                    {
                        downloadFile.RangeBegin = position;
                        int appendLength = downloadFile.AppendFileStream(fileStream);
                        await fileStream.FlushAsync();
                        fileStream.Position -= appendLength;
                    }
                    var endSecond = DateTime.Now.Second;
                    if (endSecond != beginSecond)//计算速度
                    {
                        downloadSpeed = downloadSpeed / (endSecond - beginSecond);
                        this.OnDownloadSpeedChanged((float)(downloadSpeed / 1024));
                        beginSecond = DateTime.Now.Second;
                        downloadSpeed = 0;//清空
                    }
                    this.OnDownloadProgressChanged(downloadFile.Length == null ? null : (float?)((decimal)downloadFile.RangeBegin / downloadFile.Length));
                }
            }
            catch (Exception ex)
            {
                this.OnDownloadError(ex.Message);
            }
        }
    }
}
