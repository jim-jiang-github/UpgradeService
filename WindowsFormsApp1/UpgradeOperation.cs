using Newtonsoft.Json;
using Share.Pipe;
using Share.Pipe.UpgradePackage;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public static class UpgradeOperation
    {
        private static string exePath = "Upgrade.exe";
        private static string mainMutex = "102502a8-5adb-4f05-8fba-232c52b23292";
        private static string mainExePath = "WindowsFormsApp1.exe";
        private static string upgradeMutex = "54b022e6-5069-4330-8fd6-6c2285903034";
        private static string url = "http://localhost:9099/api/UpgradeServer";
        private static string pipe = "UpgradeClient";
        private static string visible = "true";
        private static PipeClient pipeClient = null;
        static UpgradeOperation()
        {
            pipeClient = new PipeClient(pipe);
            pipeClient.ReceivePipeData += (b) =>
            {
                if (PipeUpgradeCheckVersion.TryFromBytes(b, out PipeUpgradeCheckVersion pipeCheckVersion))
                {

                    return;
                }
                if (PipeUpgradeCommand.TryFromBytes(b, out PipeUpgradeCommand pipeCommand))
                {

                    return;
                }
                if (PipeUpgradeComplete.TryFromBytes(b, out PipeUpgradeComplete pipeUpgradeComplete))
                {
                    Console.WriteLine("Success:" + pipeUpgradeComplete.Success);
                    return;
                }
                if (PipeUpgradeContent.TryFromBytes(b, out PipeUpgradeContent pipeUpgradeContent))
                {
                    Console.WriteLine("Content:" + pipeUpgradeContent.Content);
                    return;
                }
                if (PipeUpgradeError.TryFromBytes(b, out PipeUpgradeError pipeUpgradeError))
                {
                    Console.WriteLine("Error:" + pipeUpgradeError.Error);
                    return;
                }
                if (PipeUpgradeMessage.TryFromBytes(b, out PipeUpgradeMessage pipeUpgradeMessage))
                {
                    Console.WriteLine("Msg:" + pipeUpgradeMessage.Message);
                    return;
                }
                if (PipeUpgradeProgress.TryFromBytes(b, out PipeUpgradeProgress pipeUpgradeProgress))
                {
                    Console.WriteLine("R:" + pipeUpgradeProgress.Read + "|L:" + pipeUpgradeProgress.Loaded + "|T:" + pipeUpgradeProgress.Total);
                    return;
                }
                //if (PipeUpgradeProgress.TryFromBytes(b, out PipeUpgradeProgress pipeUpgradeProgress))
                //{
                //    Console.WriteLine("R:" + pipeUpgradeProgress.Read + "|L:" + pipeUpgradeProgress.Loaded + "|T:" + pipeUpgradeProgress.Total);
                //}
            };
        }
        /// <summary>
        /// 根据版本号判断是否需要升级
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static async Task<bool> Upgradeable(string version)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url + "/upgradeable?version=" + version);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        bool upgradeable = JsonConvert.DeserializeObject<bool>(await httpResponseMessage.Content.ReadAsStringAsync());
                        return upgradeable;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return false;
        }
        /// <summary>
        /// 发送校验版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static async Task<bool> CheckVersion(string version)
        {
            try
            {
                await pipeClient.Send(new PipeUpgradeCheckVersion() { Version = version }.Serialize());
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 发送校验版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static async Task<bool> UpgradeUI()
        {
            try
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Image image = global::WindowsFormsApp1.Properties.Resources.upgradeImage;
                    image.Save(memoryStream, image.RawFormat);
                    await pipeClient.Send(new PipeUpgradeUI()
                    {
                        UIImage = memoryStream.ToArray(),
                        ColorHex = System.Drawing.ColorTranslator.ToHtml(Color.FromArgb(58, 58, 58)),
                        Width = image.Width,
                        Height = image.Height,
                        Version = assembly.GetName().Version.ToString(),
                        Company = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute))).Company,
                        Copyright = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute))).Copyright
                    }.Serialize());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 启动升级客户端
        /// </summary>
        /// <returns>是否已经启动</returns>
        public static async Task<bool> StartUpgradeClient()
        {
            if (!File.Exists(exePath)) { return false; }
            if (Mutex.TryOpenExisting(upgradeMutex, out Mutex check)) //如果已经存在锁，则不打开，返回false
            {
                check?.Dispose();
                return true;
            }
            else
            {
                //如果不存在锁，则打开程序并等待程序完全打开
                Process.Start(exePath, " -v " + visible + " -u " + url + " -pipe " + pipe + " -um " + upgradeMutex + " -mm " + mainMutex + " -mr " + mainExePath);
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(5000);
                return await Task<bool>.Run(async () =>
                {
                    while (!cancellationTokenSource.IsCancellationRequested)
                    {
                        if (Mutex.TryOpenExisting(upgradeMutex, out Mutex m))
                        {
                            m?.Dispose();
                            break;
                        }
                        await Task.Delay(100);
                    }
                    return !cancellationTokenSource.IsCancellationRequested;
                }, cancellationTokenSource.Token);
            }
        }
        /// <summary>
        /// 启动升级客户端并连接
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> StartUpgradeClientWithConnect()
        {
            return await StartUpgradeClient() && await pipeClient.Connect(1000);
        }
        ///// <summary>
        ///// 关闭客户端
        ///// </summary>
        ///// <returns>是否已经关闭</returns>
        //public static async Task<bool> CloseUpgradeClient()
        //{
        //    mutex.ReleaseMutex();
        //    mutex.Dispose();
        //    mutex = new Mutex(true, "102502a8-5adb-4f05-8fba-232c52b23292", out bool flag);
        //    if (flag)
        //    {
        //    }
        //    else
        //    {
        //    }
        //    if (Mutex.TryOpenExisting(upgradeMutex, out Mutex check)) //如果已经存在锁，则不打开，返回false
        //    {
        //        check?.Dispose();
        //        //如果不存在锁，则打开程序并等待程序完全打开
        //        while (true)
        //        {
        //            await pipeClient.Send(new PipeUpgradeCommand() { Command = PipeUpgradeCommand.Commands.Colse }.Serialize());
        //            if (Mutex.TryOpenExisting(upgradeMutex, out Mutex m))
        //            {
        //                m?.Dispose();
        //                await Task.Delay(100);
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
    }
}
