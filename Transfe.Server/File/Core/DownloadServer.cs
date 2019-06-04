using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Transfe.Server.File.Entities;

namespace Transfe.Server.File.Core
{
    /// <summary> 服务端下载
    /// </summary>
    public class DownloadServer
    {
        private string downloadRoot = string.Empty;
        /// <summary>
        /// 服务端下载
        /// </summary>
        /// <param name="downloadRoot">下载目录</param>
        public DownloadServer(string downloadRoot)
        {
            this.downloadRoot = downloadRoot;
        }
        private static string baseDirector = AppContext.BaseDirectory;
        #region const
        private const int BufferSize = 80 * 1024;
        #endregion
        /// <summary> 下载根目录
        /// </summary>
        private string DownloadRoot
        {
            get
            {
                string downloadRoot = Path.Combine(baseDirector, this.downloadRoot);
                if (!Directory.Exists(downloadRoot))
                {
                    Directory.CreateDirectory(downloadRoot);
                }
                return downloadRoot;
            }
        }
        /// <summary> 通过文件名获取服务器上的文件路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetServerFilePath(string fileName)
        {
            return Path.Combine(DownloadRoot, fileName);
        }
        /// <summary> 获取文件分块信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private PartialFileInfo GetPartialFileInfo(HttpRequest request, string filePath)
        {
            PartialFileInfo partialFileInfo = new PartialFileInfo(filePath);
            if (RangeHeaderValue.TryParse(request.Headers[HeaderNames.Range].ToString(), out RangeHeaderValue rangeHeaderValue))
            {
                var range = rangeHeaderValue.Ranges.FirstOrDefault();
                if (range.From.HasValue && range.From < 0 || range.To.HasValue && range.To > partialFileInfo.FileLength - 1)
                {
                    return null;
                }
                var from = range.From;
                var to = range.To;
                if (from.HasValue)
                {
                    if (from.Value >= partialFileInfo.FileLength)
                    {
                        return null;
                    }
                    if (!to.HasValue || to.Value >= partialFileInfo.FileLength)
                    {
                        to = partialFileInfo.FileLength - 1;
                    }
                }
                else
                {
                    if (to.Value == 0)
                    {
                        return null;
                    }
                    var bytes = Math.Min(to.Value, partialFileInfo.FileLength);
                    from = partialFileInfo.FileLength - bytes;
                    to = from + bytes - 1;
                }
                partialFileInfo.IsPartial = true;
                partialFileInfo.Length = to.Value - from.Value + 1;
            }
            return partialFileInfo;
        }
        /// <summary> 获取分块文件流
        /// </summary>
        /// <param name="partialFileInfo"></param>
        /// <returns></returns>
        private Stream GetPartialFileStream( PartialFileInfo partialFileInfo)
        {
            return new PartialFileStream(partialFileInfo.FilePath, partialFileInfo.From, partialFileInfo.To);
        }
        /// <summary>
        /// 设置响应头信息
        /// </summary>
        /// <param name="response"></param>
        /// <param name="partialFileInfo"></param>
        /// <param name="fileLength"></param>
        /// <param name="fileName"></param>
        private void SetResponseHeaders(HttpResponse response, PartialFileInfo partialFileInfo)
        {
            response.Headers[HeaderNames.AcceptRanges] = "bytes";
            response.StatusCode = partialFileInfo.IsPartial ? StatusCodes.Status206PartialContent : StatusCodes.Status200OK;

            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(partialFileInfo.Name);
            response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
            response.Headers[HeaderNames.ContentType] = "application/octet-stream";
            //response.Headers[HeaderNames.ContentMD5] = partialFileInfo.MD5;
            response.Headers[HeaderNames.ContentLength] = partialFileInfo.Length.ToString();
            if (partialFileInfo.IsPartial)
            {
                response.Headers[HeaderNames.ContentRange] = new ContentRangeHeaderValue(partialFileInfo.From, partialFileInfo.To, partialFileInfo.FileLength).ToString();
            }
        }
        /// <summary> 异步获取下载流
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task<Stream> GetDownloadStreamAsync(HttpContext httpContext, string fileName)
        {
            return Task.Run<Stream>(() =>
            {
                string filePath = this.GetServerFilePath(fileName);
                PartialFileInfo partialFileInfo = this.GetPartialFileInfo(httpContext.Request, filePath);
                this.SetResponseHeaders(httpContext.Response, partialFileInfo);
                return this.GetPartialFileStream(partialFileInfo);
            });
        }
        /// <summary> 获取下载流
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Stream GetDownloadStream(HttpContext httpContext, string fileName)
        {
            string filePath = this.GetServerFilePath(fileName);
            PartialFileInfo partialFileInfo = this.GetPartialFileInfo(httpContext.Request, filePath);
            this.SetResponseHeaders(httpContext.Response, partialFileInfo);
            return this.GetPartialFileStream(partialFileInfo);
        }
    }
}
