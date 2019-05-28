using FileTransfe.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfe.Core
{
    /// <summary> 服务端下载
    /// </summary>
    public class ServerUpload
    {
        private string uploadRoot = string.Empty;
        public ServerUpload(string uploadRoot)
        {
            this.uploadRoot = uploadRoot;
        }
        private static string baseDirector = AppContext.BaseDirectory;
        #region const
        private const int BufferSize = 80 * 1024;
        #endregion
        /// <summary> 上传跟目录
        /// </summary>
        private string UploadRoot
        {
            get
            {
                string uploadRoot = Path.Combine(baseDirector, this.uploadRoot);
                if (!Directory.Exists(uploadRoot))
                {
                    Directory.CreateDirectory(uploadRoot);
                }
                return uploadRoot;
            }
        }
        /// <summary> 异步获取下载流
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task<RespondResult> Upload(HttpRequest httpRequest)
        {
            return Task<RespondResult>.Run(() =>
            {
                try
                {
                    List<RespondResult> error = new List<RespondResult>();
                    foreach (var formFile in httpRequest.Form.Files)
                    {
                        RespondResult respondResult = null;
                        if (formFile.Name == "file")
                        {
                            respondResult = this.UploadOnce(formFile);
                        }
                        else
                        {
                            respondResult = this.UploadChunk(formFile);
                        }
                        if (!respondResult.Result)
                        {
                            error.Add(respondResult);
                        }
                    }
                    if (error.Count == 0)
                    {
                        return new RespondResult()
                        {
                            Message = "上传成功"
                        };
                    }
                    else
                    {
                        return new RespondResult()
                        {
                            Result = false,
                            Message = "上传失败",
                            Details = error.Select(e => e.Message).ToArray()
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new RespondResult()
                    {
                        Result = false,
                        Message = "上传失败" + ex.Message
                    };
                }
            });
        }
        /// <summary> 上传
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private RespondResult UploadOnce(IFormFile formFile)
        {
            try
            {
                string filePath = Path.Combine(this.UploadRoot, formFile.FileName);
                if (!File.Exists(filePath))
                {
                    using (Stream stream = File.Create(filePath))
                    {
                        formFile.CopyTo(stream);
                    }
                    return new RespondResult()
                    {
                        Message = "上传成功"
                    };
                }
                return new RespondResult()
                {
                    Message = "已经存在无需上传"
                };
            }
            catch (Exception ex)
            {
                return new RespondResult()
                {
                    Result = false,
                    Message = formFile.FileName + ":上传失败" + ex.Message
                };
            }
        }
        /// <summary> 分块上传
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private RespondResult UploadChunk(IFormFile formFile)
        {
            try
            {
                string dir = Path.Combine(this.UploadRoot, formFile.FileName + "_Merge");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string chunkPath = Path.Combine(dir, formFile.Name);
                if (!File.Exists(chunkPath))
                {
                    using (Stream stream = File.Create(chunkPath))
                    {
                        formFile.CopyTo(stream);
                    }
                    return new RespondResult()
                    {
                        Message = "上传成功"
                    };
                }
                return new RespondResult()
                {
                    Message = "已经存在无需上传"
                };
            }
            catch (Exception ex)
            {
                return new RespondResult()
                {
                    Result = false,
                    Message = formFile.FileName + ":的分块" + formFile.Name + " 上传失败" + ex.Message
                };
            }
        }
        /// <summary>
        /// 合并分块
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task<RespondResult> Merge(string fileName)
        {
            return Task<RespondResult>.Run(() =>
            {
                string dir = Path.Combine(this.UploadRoot, fileName + "_Merge");
                if (!Directory.Exists(dir))
                {
                    return new RespondResult()
                    {
                        Result = false,
                        Message = "不存在合并的文件" + fileName
                    };
                }
                string filePath = Path.Combine(this.UploadRoot, fileName);
                try
                {
                    string[] files = Directory.GetFiles(dir).OrderBy(p => Convert.ToInt32(Path.GetFileNameWithoutExtension(p))).ToArray();
                    using (Stream writeStream = File.Create(filePath))
                    {
                        for (int i = 0; i < files.Length; i++)
                        {
                            using (FileStream chunk = File.OpenRead(files[i]))
                            {
                                byte[] buffer = new byte[1024];
                                int readLength = chunk.Read(buffer, 0, buffer.Length);
                                while (readLength > 0)
                                {
                                    writeStream.Write(buffer, 0, readLength);
                                    readLength = chunk.Read(buffer, 0, buffer.Length);
                                }
                            }
                        }
                    }
                    return new RespondResult()
                    {
                        Message = "合并成功"
                    };
                }
                catch (Exception ex)
                {
                    return new RespondResult()
                    {
                        Result = false,
                        Message = "文件合并出错！请重新上传！"
                    };
                }
                finally
                {
                    Directory.Delete(dir, true);
                }
            });
        }
        ///// <summary> 通过文件名获取服务器上的文件路径
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
        //private static string GetServerFilePath(this string fileName)
        //{
        //    return Path.Combine(DownloadRoot, fileName);
        //}
        ///// <summary> 获取文件分块信息
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="filePath"></param>
        ///// <returns></returns>
        //private static PartialFileInfo GetPartialFileInfo(this HttpRequest request, string filePath)
        //{
        //    PartialFileInfo partialFileInfo = new PartialFileInfo(filePath);
        //    if (RangeHeaderValue.TryParse(request.Headers[HeaderNames.Range].ToString(), out RangeHeaderValue rangeHeaderValue))
        //    {
        //        var range = rangeHeaderValue.Ranges.FirstOrDefault();
        //        if (range.From.HasValue && range.From < 0 || range.To.HasValue && range.To > partialFileInfo.FileLength - 1)
        //        {
        //            return null;
        //        }
        //        var from = range.From;
        //        var to = range.To;
        //        if (from.HasValue)
        //        {
        //            if (from.Value >= partialFileInfo.FileLength)
        //            {
        //                return null;
        //            }
        //            if (!to.HasValue || to.Value >= partialFileInfo.FileLength)
        //            {
        //                to = partialFileInfo.FileLength - 1;
        //            }
        //        }
        //        else
        //        {
        //            if (to.Value == 0)
        //            {
        //                return null;
        //            }
        //            var bytes = Math.Min(to.Value, partialFileInfo.FileLength);
        //            from = partialFileInfo.FileLength - bytes;
        //            to = from + bytes - 1;
        //        }
        //        partialFileInfo.IsPartial = true;
        //        partialFileInfo.Length = to.Value - from.Value + 1;
        //    }
        //    return partialFileInfo;
        //}
        ///// <summary> 获取分块文件流
        ///// </summary>
        ///// <param name="partialFileInfo"></param>
        ///// <returns></returns>
        //private static Stream GetPartialFileStream(this PartialFileInfo partialFileInfo)
        //{
        //    return new PartialFileStream(partialFileInfo.FilePath, partialFileInfo.From, partialFileInfo.To);
        //}
        ///// <summary>
        ///// 设置响应头信息
        ///// </summary>
        ///// <param name="response"></param>
        ///// <param name="partialFileInfo"></param>
        ///// <param name="fileLength"></param>
        ///// <param name="fileName"></param>
        //private static void SetResponseHeaders(this HttpResponse response, PartialFileInfo partialFileInfo)
        //{
        //    response.Headers[HeaderNames.AcceptRanges] = "bytes";
        //    response.StatusCode = partialFileInfo.IsPartial ? StatusCodes.Status206PartialContent : StatusCodes.Status200OK;

        //    var contentDisposition = new ContentDispositionHeaderValue("attachment");
        //    contentDisposition.SetHttpFileName(partialFileInfo.Name);
        //    response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
        //    response.Headers[HeaderNames.ContentType] = "application/octet-stream";
        //    //response.Headers[HeaderNames.ContentMD5] = partialFileInfo.MD5;
        //    response.Headers[HeaderNames.ContentLength] = partialFileInfo.Length.ToString();
        //    if (partialFileInfo.IsPartial)
        //    {
        //        response.Headers[HeaderNames.ContentRange] = new ContentRangeHeaderValue(partialFileInfo.From, partialFileInfo.To, partialFileInfo.FileLength).ToString();
        //    }
        //}
        ///// <summary> 获取下载流
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
        //public static Task<Stream> GetDownloadStreamAsync(HttpContext httpContext, string fileName)
        //{
        //    return Task.Run<Stream>(() =>
        //    {
        //        string filePath = fileName.GetServerFilePath();
        //        PartialFileInfo partialFileInfo = httpContext.Request.GetPartialFileInfo(filePath);
        //        httpContext.Response.SetResponseHeaders(partialFileInfo);
        //        return partialFileInfo.GetPartialFileStream();
        //    });
        //}
        ///// <summary> 获取下载流
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
        //public static Stream GetDownloadStream(HttpContext httpContext, string fileName)
        //{
        //    string filePath = fileName.GetServerFilePath();
        //    PartialFileInfo partialFileInfo = httpContext.Request.GetPartialFileInfo(filePath);
        //    httpContext.Response.SetResponseHeaders(partialFileInfo);
        //    return partialFileInfo.GetPartialFileStream();
        //}
    }
}
