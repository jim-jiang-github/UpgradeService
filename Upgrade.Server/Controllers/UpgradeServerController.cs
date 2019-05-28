using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FileTransfe.Core;
using FileTransfe.Entities;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;
using System.Net.Http;

namespace Upgrade.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpgradeServerController : ControllerBase
    {
        /// <summary>
        /// 用文件名获取下载链接
        /// </summary>
        /// <remarks>
        /// 例子:
        /// Get api/UpgradeService/download?fileName=xxxxxxxxxxxx
        /// </remarks>
        /// <param name="fileName">文件名</param>
        /// <returns>文件下载请求结果</returns> 
        /// <response code="201">返回value字符串</response>
        /// <response code="400">如果id为空</response>  
        // GET api/values/2
        [HttpGet("download")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public Task<IActionResult> Download(string fileName)
        {
            return Task.Run<IActionResult>(() =>
            {
                Stream stream = ServerDownload.GetDownloadStream(this.HttpContext, fileName);
                return File(stream, this.HttpContext.Response.Headers[HeaderNames.ContentType].ToString(), true);
            });
        }
        [HttpPost("upload")]
        public Task<IActionResult> Upload(List<IFormFile> iFormFiles)
        { //这里一定要iFormFiles这个参数，不然Request.Form.Files就获取不到了
            var date = Request;
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {


                }
            }
            Stream body = Request.Body;
            var filename = Path.GetTempFileName();
            using (FileStream fs = System.IO.File.Open(filename, FileMode.OpenOrCreate))
            {
                body.CopyTo(fs);
            }

            return null;
        }

        //[HttpPost("upgradeRelease")]
        //public Task<IActionResult> UpgradeRelease(string version)
        //{

        //}
        //[HttpPost("upgradeReleasing")]
        //public Task<IActionResult> UpgradeReleasing(string version)
        //{

        //}
        //[HttpPost("upgradeReleaseComplete")]
        //public Task<IActionResult> UpgradeReleaseComplete(string version)
        //{

        //}
    }
}