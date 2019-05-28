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
    /// <summary> 客户端下载
    /// </summary>
    public static class ClientDownload
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async void Download(string downloadPartPath, DownloadMsg downloadMsg = null)
        {
            using (FileStream fileStream = File.Open(downloadPartPath, FileMode.Open))
            using (HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) })
            {
                DownloadFile downloadFile = DownloadFile.FromFileStream(fileStream);
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
                    await Download(stream, fileStream, downloadFile, downloadMsg);
                    if (md5 == null || md5 == MD5Tools.GetFileMd5(downloadPartPath))
                    {
                        downloadMsg.Complete = true;
                    }
                }
                catch (Exception ex)
                {
                    //请求错误
                }
            }
            downloadMsg.Complete = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async void Download(Uri uri, string downloadPath, DownloadMsg downloadMsg = null)
        {
            string downloadPartPath = Path.Combine(downloadPath + ".downloadPart");
            using (FileStream fileStream = File.Create(downloadPartPath))
            using (HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) })
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Range = new RangeHeaderValue(0, null);
                    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
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
                        URL = uri.AbsoluteUri
                    };
                    await Download(stream, fileStream, downloadFile, downloadMsg);
                    if (md5 == null || md5 == MD5Tools.GetFileMd5(downloadPartPath))
                    {
                        downloadMsg.Complete = true;
                    }
                }
                catch (Exception ex)
                {
                    //请求错误
                }
            }
            downloadMsg.Complete = false;
        }
        private static async Task Download(Stream downloadStream, FileStream fileStream, DownloadFile downloadFile, DownloadMsg downloadMsg)
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
                        if (downloadMsg != null)
                        {
                            downloadMsg.Speed = (float)(downloadSpeed / 1024);
                        }
                        beginSecond = DateTime.Now.Second;
                        downloadSpeed = 0;//清空
                    }
                    if (downloadMsg != null)
                    {
                        downloadMsg.Progress = downloadFile.Length == null ? null : (float?)((decimal)downloadFile.RangeBegin / downloadFile.Length);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
