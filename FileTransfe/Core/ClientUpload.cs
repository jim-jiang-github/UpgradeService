using FileTransfe.Entities;
using FileTransfe.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileTransfe.Core
{
    /// <summary>
    /// 上传进度变化事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="progress">进度</param>
    public delegate void UploadProgressChangedHandler(object sender, float progress);
    /// <summary>
    /// 上传速度变化事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="speed">速度</param>
    public delegate void UploadSpeedChangedHandler(object sender, float speed);
    /// <summary>
    /// 上传完成事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="result"></param>
    public delegate void UploadCompletedHandler(object sender, bool done, string result);
    /// <summary>
    /// 上传错误事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="result"></param>
    public delegate void UploadErrorHandler(object sender, string result);
    /// <summary> 客户端上传
    /// </summary>
    public class ClientUpload
    {
        #region 事件
        /// <summary>
        /// 上传进度变化事件
        /// </summary>
        public event UploadProgressChangedHandler UploadProgressChanged;
        protected virtual void OnUploadProgressChanged(float progress)
        {
            this.UploadProgressChanged?.Invoke(this, progress);
        }
        /// <summary>
        /// 上传速度变化事件
        /// </summary>
        public event UploadSpeedChangedHandler UploadSpeedChanged;
        protected virtual void OnUploadSpeedChanged(float speed)
        {
            this.UploadSpeedChanged?.Invoke(this, speed);
        }
        /// <summary>
        /// 上传完成事件
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="speed"></param>
        public event UploadCompletedHandler UploadCompleted;
        protected virtual void OnUploadCompleted(bool done, string result = null)
        {
            this.UploadCompleted?.Invoke(this, done, result);
        }
        /// <summary>
        /// 上传错误事件
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="speed"></param>
        public event UploadErrorHandler UploadError;
        protected virtual void OnUploadError(string result)
        {
            this.UploadError?.Invoke(this, result);
        }
        #endregion
        #region const
        private const int UploadBufferSize = 4096;
        #endregion
        #region 属性
        /// <summary>
        /// 分块大小
        /// </summary>
        public int ChunkSize { get; set; } = 1024 * 1024 * 2; //分块大小2M
        /// <summary> 上传的url
        /// </summary>
        private string UploadUrl { get; set; }
        /// <summary> 分块合并的url(可以为空，如果你不分块上传)
        /// </summary>
        private string MergeURL { get; set; }
        #endregion
        public ClientUpload(string uploadUrl, string mergeURL = null)
        {
            this.UploadUrl = uploadUrl;
            this.MergeURL = mergeURL;
        }
        public void UploadAsync(string filePath)
        {
            Task.Run(async () =>
            {
                FileInfo fileInfo = new FileInfo(filePath);
                long length = fileInfo.Length;
                if (length > ChunkSize)
                {
                    this.OnUploadCompleted(await UploadLarge(filePath));
                }
                else
                {
                    this.OnUploadCompleted(await UploadOnce(filePath));
                }
            });
        }
        /// <summary>
        /// 一次性上传
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task<bool> UploadOnce(string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            using (HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false }))//若想手动设置Cookie则必须设置UseCookies = false
            {
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                multipartFormDataContent.Add(new ProgressableStreamContent(fileStream, UploadBufferSize, (u) => this.OnUploadProgressChanged((float)((decimal)u / fileStream.Length)), (s) => this.OnUploadSpeedChanged(s)), "file", Path.GetFileName(filePath));
                try
                {
                    var result = await client.PostAsync(new Uri(this.UploadUrl), multipartFormDataContent);
                    if (result.IsSuccessStatusCode)
                    {
                        RespondResult respondResult = JsonConvert.DeserializeObject<RespondResult>(await result.Content.ReadAsStringAsync());
                        if (!respondResult.Result)
                        {
                            this.OnUploadError(respondResult.Message);
                        }
                        return respondResult.Result;
                    }
                }
                catch (Exception ex)
                {
                    this.OnUploadError(ex.Message);
                }
            }
            return false;
        }
        /// <summary>
        /// 上传大文件
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task<bool> UploadLarge(string filePath)
        {
            try
            {
                bool success = true;
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    long uploaded = 0;
                    string chunkPath = Path.Combine("Merge", "chunk");
                    for (long i = 0; i < fileStream.Length; i += ChunkSize)
                    {
                        fileStream.Position = i;
                        using (Stream writeStream = File.OpenWrite(chunkPath))
                        {
                            byte[] buffer = new byte[ChunkSize];
                            int readLength = fileStream.Read(buffer, 0, buffer.Length);
                            writeStream.Write(buffer, 0, readLength);
                        }
                        using (Stream readStream = File.OpenRead(chunkPath))
                        {
                            long readLength = readStream.Length;
                            if (!await UploadChunk(readStream, filePath, (i / ChunkSize).ToString(), uploaded, fileStream.Length))
                            {
                                success = false;
                                break;
                            }
                            uploaded += readLength;
                        }
                    }
                    try
                    {
                        File.Delete(chunkPath);
                    }
                    catch (Exception ex)
                    {
                        this.OnUploadError(ex.Message);
                    }
                }
                if (success && await this.Merge(filePath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.OnUploadError(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 上传分块
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="stream"></param>
        /// <param name="filePath"></param>
        /// <param name="chunkName"></param>
        /// <param name="tryCount"></param>
        /// <returns></returns>
        private async Task<bool> UploadChunk(Stream stream, string filePath, string chunkName, long uploaded, long totalLength, int tryCount = 0)
        {
            bool success = false;
            using (HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false }))//若想手动设置Cookie则必须设置UseCookies = false
            {
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                multipartFormDataContent.Add(new ProgressableStreamContent(stream, UploadBufferSize, (u) =>
                {
                    this.OnUploadProgressChanged((float)((decimal)(uploaded + u) / totalLength));
                }, (s) => this.OnUploadSpeedChanged(s)), chunkName, Path.GetFileName(filePath));
                try
                {
                    var result = await client.PostAsync(new Uri(this.UploadUrl), multipartFormDataContent);
                    if (result.IsSuccessStatusCode)
                    {
                        RespondResult respondResult = JsonConvert.DeserializeObject<RespondResult>(await result.Content.ReadAsStringAsync());
                        success = respondResult.Result;
                        if (!respondResult.Result)
                        {
                            this.OnUploadError(respondResult.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.OnUploadError(ex.Message);
                }
            }
            if (!success && tryCount < 3)
            {
                this.OnUploadProgressChanged((float)((decimal)uploaded / totalLength));
                success |= await UploadChunk(stream, filePath, chunkName, uploaded, totalLength, tryCount + 1);
            }
            return success;
        }
        /// <summary>
        /// 合并上传的分块
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task<bool> Merge(string filePath)
        {
            using (HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) })
            {
                try
                {
                    var result = await httpClient.GetAsync(this.MergeURL + "?fileName=" + Path.GetFileName(filePath));
                    if (result.IsSuccessStatusCode)
                    {
                        RespondResult respondResult = JsonConvert.DeserializeObject<RespondResult>(await result.Content.ReadAsStringAsync());
                        if (!respondResult.Result)
                        {
                            this.OnUploadError(respondResult.Message);
                        }
                        return respondResult.Result;
                    }
                }
                catch (Exception ex)
                {
                    this.OnUploadError(ex.Message);
                }
            }
            return false;
        }
        private class ProgressableStreamContent : StreamContent
        {
            private Stream content = null;
            private int bufferSize = 4096;
            private Action<long> uploaded = null;
            private Action<float> speed = null;
            public ProgressableStreamContent(Stream content, int bufferSize, Action<long> uploaded, Action<float> speed) : base(content, bufferSize)
            {
                this.content = content;
                this.bufferSize = bufferSize;
                this.uploaded = uploaded;
                this.speed = speed;
            }

            protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                return Task.Run(() =>
                {
                    var buffer = new Byte[this.bufferSize];
                    long upload = 0;
                    decimal downloadSpeed = 0;//下载速度
                    var beginSecond = DateTime.Now.Second;//当前时间秒
                    using (content) while (true)
                        {
                            var length = content.Read(buffer, 0, buffer.Length);
                            if (length <= 0) break;
                            upload += length;
                            downloadSpeed += length;
                            stream.Write(buffer, 0, length);
                            var endSecond = DateTime.Now.Second;
                            if (endSecond != beginSecond)//计算速度
                            {
                                downloadSpeed = downloadSpeed / (endSecond - beginSecond);
                                this.speed?.Invoke((float)(downloadSpeed / 1024));
                                beginSecond = DateTime.Now.Second;
                                downloadSpeed = 0;//清空
                            }
                            this.uploaded?.Invoke(upload);
                        }
                });
                //return base.SerializeToStreamAsync(stream, context);
            }
        }
    }
}
