using FileTransfe.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Upgrade.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpgradeServerController : Controller
    {
        private ServerDownload serverDownload = new ServerDownload("Upgrade");
        private ServerUpload serverUpload = new ServerUpload("Upgrade");
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
        public async Task<IActionResult> Download(string fileName)
        {
            Stream stream = await serverDownload.GetDownloadStreamAsync(this.HttpContext, fileName);
            return File(stream, this.HttpContext.Response.Headers[HeaderNames.ContentType].ToString(), true);
        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        { 
            //这里一定要formFiles这个参数，不然Request.Form.Files就获取不到了(后来发现有时可以有时不行，是.netcore的bug？)
            //发现有的时候name必须为“file”才可以，其他的就不行了
            //后来发现都不是 只是调试的时候过不了而已
            return Json(await serverUpload.Upload(this.Request));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">合并的文件名</param>
        /// <returns></returns>
        [HttpGet("merge")]
        public async Task<IActionResult> Merge(string fileName)
        {
            return Json(await serverUpload.Merge(fileName));
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