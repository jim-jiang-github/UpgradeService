using Share.Pipe;
using Share.Pipe.UpgradePackage;
using Share.Upgrade.Json;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgrade.Program.Client
{
    public class CommandPipe : Command
    {
        private PipeServer pipeServer = null;
        public override string Name => "Pipe管道";
        public override string Code => "-pipe";
        public override string Descript => "主要用于进程之间的通讯，比如获取升级信息、进度等等";
        public void Listen()
        {
            if (this.Value == null) { return; }
            this.pipeServer = new PipeServer(this.Value);
            this.pipeServer.Listen();
            this.pipeServer.ReceivePipeData += async (b) =>
            {
                //if (PipeUpgradeCommand.TryFromBytes(b, out PipeUpgradeCommand pipeCommand))
                //{
                //    switch (pipeCommand.Command)
                //    {
                //        case PipeUpgradeCommand.Commands.Colse:
                //            Application.Exit();
                //            break;
                //    }
                //    return;
                //}
                if (PipeUpgradeUI.TryFromBytes(b, out PipeUpgradeUI pipeUpgradeUI))
                {
                    if (pipeUpgradeUI.UIImage != null)
                    {
                        try
                        {
                            using (MemoryStream ms = new MemoryStream(pipeUpgradeUI.UIImage))
                            {
                                UpgradeApplication.SetUIRenderer(new UpgradeUIRenderer()
                                {
                                    BackgroundImage = Bitmap.FromStream(ms),
                                    BackgroundColor = System.Drawing.ColorTranslator.FromHtml(pipeUpgradeUI.ColorHex),
                                    Size = new Size(pipeUpgradeUI.Width, pipeUpgradeUI.Height),
                                    Company = pipeUpgradeUI.Company,
                                    Copyright = pipeUpgradeUI.Copyright,
                                    Version = pipeUpgradeUI.Version
                                });
                            }
                        }
                        catch (Exception ex) { }
                    }
                    return;
                }
                if (PipeUpgradeCheckVersion.TryFromBytes(b, out PipeUpgradeCheckVersion pipeCheckVersion))
                {
                    JsonReleaseVersion jsonReleaseVersion = await Commands.CommandUrl.CheckVersion(pipeCheckVersion.Version);
                    if (jsonReleaseVersion != null)
                    {
                        if (await Commands.CommandUrl.Upgrade(jsonReleaseVersion))
                        {
                            UpgradeApplication.SetMessage("升级完成");
                        }
                        else
                        {
                            UpgradeApplication.SetMessage("升级失败");
                        }
                    }
                    else
                    {
                        UpgradeApplication.SetMessage("无法获取升级信息");
                    }
                    await Task.Delay(1000);
                    if (await Commands.CommandMainRestart.MainStart())
                    {
                        Application.Exit();
                    }
                    else
                    {
                        UpgradeApplication.SetMessage("无法打开主程序");
                        await Task.Delay(1000);
                        Application.Exit();
                    }
                }
            };
        }
        public async Task SendComplete(bool success)
        {
            await pipeServer.Send(new PipeUpgradeComplete() { Success = success }.Serialize());
        }
        public async Task SendError(string error)
        {
            await pipeServer.Send(new PipeUpgradeError() { Error = error }.Serialize());
        }
        public async Task SendMessage(string message)
        {
            await pipeServer.Send(new PipeUpgradeMessage() { Message = message }.Serialize());
        }
        public async Task SendProgress(int read, long loaded, long total)
        {
            await pipeServer.Send(new PipeUpgradeProgress() { Read = read, Loaded = loaded, Total = total }.Serialize());
        }
        public async Task SendUpgradeContent(string upgradeContent)
        {
            await pipeServer.Send(new PipeUpgradeContent() { Content = upgradeContent }.Serialize());
        }
    }
}
