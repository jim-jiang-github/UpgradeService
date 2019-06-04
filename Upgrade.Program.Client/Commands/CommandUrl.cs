using Newtonsoft.Json;
using Share.Pipe;
using Share.Transfe.Args;
using Share.Transfe.Delegates;
using Share.Transfe.File.Core;
using Share.Transfe.Tools;
using Share.Upgrade.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgrade.Program.Client
{
    public class CommandUrl : Command
    {
        private DownloadClient downloadClient = null;
        private long readed = 0;
        private long total = 0;
        public override string Name => "升级服务器地址";
        public override string Code => "-u";
        public override string Descript => "用于与服务器连接";
        public override string Value
        {
            get => base.Value;
            set
            {
                base.Value = value;
                this.downloadClient = new DownloadClient(value + "/download");
                this.downloadClient.DownloadCompleted += async (s, e) =>
                {
                    await Commands.CommandPipe.SendComplete(e.Success);
                };
                this.downloadClient.DownloadError += async (s, e) =>
                {
                    UpgradeApplication.SetMessage(e.Error);
                    await Commands.CommandPipe.SendError(e.Error);
                };
                this.downloadClient.DownloadProgressChanged += async (s, e) =>
                {
                    readed += e.Read;
                    UpgradeApplication.SetProgress((float)((decimal)readed / total));
                    await Commands.CommandPipe.SendProgress(e.Read, e.Loaded, total);
                };
            }
        }
        /// <summary> 校验版本，并获取最小版本的信息
        /// </summary>
        /// <param name="version">版本号</param>
        public async Task<JsonReleaseVersion> CheckVersion(string version)
        {
            if (this.Value == null) { return null; }
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(this.Value + "/checkVersion?version=" + version);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<JsonReleaseVersion>(await httpResponseMessage.Content.ReadAsStringAsync());
                    }
                }
                catch (Exception ex)
                {
                    await Commands.CommandPipe.SendError(ex.Message);
                }
            }
            return null;
        }
        /// <summary>
        /// 升级到JsonReleaseVersion的版本
        /// </summary>
        /// <param name="jsonReleaseVersion"></param>
        /// <returns></returns>
        public async Task<bool> Upgrade(JsonReleaseVersion jsonReleaseVersion)
        {
            await Commands.CommandPipe.SendUpgradeContent(jsonReleaseVersion.UpdateContent);
            UpgradeApplication.SetUpgradeContent(jsonReleaseVersion.UpdateContent);
            UpgradeApplication.SetVersionServer(jsonReleaseVersion.Version);
            string upgradeDownloadsDir = Path.Combine(Application.StartupPath, jsonReleaseVersion.Version);
            if (jsonReleaseVersion.Files.Length != 0)
            {
                #region 过滤掉已经存在的文件和已经下载的文件
                JsonFileDetail[] jsonFileDetails = jsonReleaseVersion.Files.Where(f =>
                {
                    string localFilePath = Path.Combine(Application.StartupPath, f.Name);
                    if (File.Exists(localFilePath) && MD5Tools.GetFileMd5(localFilePath) == f.MD5) //校验本地是否存在文件，如果存在并且版本校验通过就不下载
                    {
                        return false;
                    }
                    string upgradeDownloadsFilePath = Path.Combine(upgradeDownloadsDir, f.Name);
                    if (File.Exists(upgradeDownloadsFilePath) && MD5Tools.GetFileMd5(upgradeDownloadsFilePath) == f.MD5) //校验本地是否存在文件，如果存在并且版本校验通过就不下载
                    {
                        return false;
                    }
                    return true;
                }).ToArray();
                #endregion
                #region 计算剩余应该下载的大小
                this.total = jsonFileDetails.Sum(f => f.Length);
                this.readed = jsonFileDetails.Sum(f =>
                {
                    string downloadPath = Path.Combine(upgradeDownloadsDir, f.Name);
                    string downloadPartPath = downloadPath + ".downloadPart";
                    if (File.Exists(downloadPartPath))
                    {
                        return new FileInfo(downloadPartPath).Length;
                    }
                    else { return 0; }
                });
                #endregion
                foreach (JsonFileDetail jsonFileDetail in jsonFileDetails)
                {
                    UpgradeApplication.SetMessage("正在下载：" + jsonFileDetail.Name);
                    if (!await this.Download(upgradeDownloadsDir, jsonFileDetail))
                    {
                        return false;
                    }
                }
            }
            if (jsonReleaseVersion.Deletes.Length != 0)
            {
                foreach (string deleteFileName in jsonReleaseVersion.Deletes)
                {
                    UpgradeApplication.SetMessage("正在删除：" + deleteFileName);
                    if (!await this.Delete(deleteFileName))
                    {
                        return false;
                    }
                }
            }
            return await this.Release(upgradeDownloadsDir);
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="upgradeDownloadsDir"></param>
        /// <param name="jsonFileDetail"></param>
        /// <param name="tryCount"></param>
        /// <returns></returns>
        private async Task<bool> Download(string upgradeDownloadsDir, JsonFileDetail jsonFileDetail, int tryCount = 0)
        {
            string downloadPath = Path.Combine(upgradeDownloadsDir, jsonFileDetail.Name);
            string downloadPartPath = downloadPath + ".downloadPart";
            bool success = File.Exists(downloadPartPath) ? await this.downloadClient.ResumeDownload(downloadPartPath) : await this.downloadClient.UrlDownload(Path.Combine(jsonFileDetail.Version, jsonFileDetail.Name), downloadPath);
            if (success)
            {
                if (File.Exists(downloadPath) && MD5Tools.GetFileMd5(downloadPath) == jsonFileDetail.MD5)
                {
                    return true;
                }
                else
                {
                    File.Delete(downloadPath);
                }
            }
            if (tryCount < 5)
            {
                return await Download(upgradeDownloadsDir, jsonFileDetail, tryCount + 1);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除文件命令
        /// </summary>
        /// <param name="deleteFileName"></param>
        /// <param name="tryCount"></param>
        /// <returns></returns>
        private async Task<bool> Delete(string deleteFileName, int tryCount = 0)
        {
            string localFilePath = Path.Combine(Application.StartupPath, deleteFileName);
            File.Delete(localFilePath);
            if (File.Exists(localFilePath))
            {
                if (tryCount < 5)
                {
                    await Task.Delay(1000);
                    return await Delete(deleteFileName, tryCount + 1);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        private async Task<bool> Release(string upgradeDownloadsDir)
        {
            if (!Directory.Exists(upgradeDownloadsDir)) { return true; }
            string[] upgradeFiles = Directory.GetFiles(upgradeDownloadsDir, "*", SearchOption.AllDirectories);
            bool success = true;
            foreach (string upgradeFile in upgradeFiles)
            {
                string toFilePath = upgradeFile.Replace(upgradeDownloadsDir, Application.StartupPath);
                success &= await FileTools.MoveFile(upgradeFile, toFilePath);
            }
            success &= await FileTools.DeleteDirectory(upgradeDownloadsDir);
            return success;
        }
    }
}
