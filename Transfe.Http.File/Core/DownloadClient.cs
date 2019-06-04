using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Transfe.Http.File.Args;
using Transfe.Http.File.Delegates;
using Transfe.Http.File.Entities;
using Transfe.Http.File.Tools;

namespace Transfe.Http.File.Core
{
    /// <summary> 客户端下载
    /// </summary>
    public class DownloadClient
    {
        #region 事件
        /// <summary>
        /// 下载进度变化事件
        /// </summary>
        public event ProgressChangedHandler DownloadProgressChanged;
        protected virtual void OnDownloadProgressChanged(ProgressChangedArgs progressChangedArgs)
        {
            this.DownloadProgressChanged?.Invoke(this, progressChangedArgs);
        }
        /// <summary>
        /// 下载速度变化事件
        /// </summary>
        public event SpeedChangedHandler DownloadSpeedChanged;
        protected virtual void OnDownloadSpeedChanged(SpeedChangedArgs speedChangedArgs)
        {
            this.DownloadSpeedChanged?.Invoke(this, speedChangedArgs);
        }
        /// <summary>
        /// 下载完成事件
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="speed"></param>
        public event CompletedHandler DownloadCompleted;
        protected virtual void OnDownloadCompleted(CompletedArgs completedArgs)
        {
            this.DownloadCompleted?.Invoke(this, completedArgs);
        }
        /// <summary>
        /// 下载错误事件
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="speed"></param>
        public event ErrorHandler DownloadError;
        protected virtual void OnDownloadError(ErrorArgs errorArgs)
        {
            this.DownloadError?.Invoke(this, errorArgs);
        }
        #endregion
        #region 属性
        /// <summary> 下载的url
        /// </summary>
        private string DownloadUrl { get; set; }
        #endregion
        public DownloadClient(string downloadUrl)
        {
            this.DownloadUrl = downloadUrl;
        }
        /// <summary>
        /// 断点续传下载
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async void ResumeDownload(string partPath)
        {
            bool success = false;
            using (FileStream fileStream = System.IO.File.Open(partPath, FileMode.Open))
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
                        if (fileStream.Length == downloadFile.Length)
                        {
                            if (md5 == null || md5 == MD5Tools.GetFileMd5(partPath))
                            {
                                success = true;
                            }
                        }
                        else
                        {
                            success = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        //请求错误
                        this.OnDownloadError(new ErrorArgs(ex.Message));
                    }
                }
            }
            if (success)
            {
                try
                {
                    System.IO.File.Move(partPath, partPath.Replace(".downloadPart", ""));
                }
                catch (Exception ex)
                {
                    this.OnDownloadError(new ErrorArgs(ex.Message));
                }
            }
            this.OnDownloadCompleted(new CompletedArgs(success));
        }
        /// <summary>
        /// 根据url下载
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async void Download(string filePath)
        {
            bool success = false;
            string downloadPartPath = Path.Combine(filePath + ".downloadPart");
            string url = this.DownloadUrl + "?fileName=" + Path.GetFileName(filePath);
            using (FileStream fileStream = System.IO.File.Create(downloadPartPath))
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
                    if (fileStream.Length == downloadFile.Length)
                    {
                        if (md5 == null || md5 == MD5Tools.GetFileMd5(downloadPartPath))
                        {
                            success = true;
                        }
                    }
                    else
                    {
                        success = false;
                    }
                }
                catch (Exception ex)
                {
                    this.OnDownloadError(new ErrorArgs(ex.Message));
                }
            }
            if (success)
            {
                try
                {
                    System.IO.File.Move(downloadPartPath, filePath);
                }
                catch (Exception ex)
                {
                    this.OnDownloadError(new ErrorArgs(ex.Message));
                }
            }
            this.OnDownloadCompleted(new CompletedArgs(success));
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
                        this.OnDownloadSpeedChanged(new SpeedChangedArgs((float)(downloadSpeed / 1024)));
                        beginSecond = DateTime.Now.Second;
                        downloadSpeed = 0;//清空
                    }
                    this.OnDownloadProgressChanged(new ProgressChangedArgs(readLength, downloadFile.RangeBegin, downloadFile.Length == null ? -1 : downloadFile.Length.Value));
                }
            }
            catch (Exception ex)
            {
                this.OnDownloadError(new ErrorArgs(ex.Message));
            }
        }
    }
}
