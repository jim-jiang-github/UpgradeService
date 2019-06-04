using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgrade.Program.Client
{
    public class UpgradeApplication
    {
        private static FmUpgrade ui = new FmUpgrade()
        {
            UIRenderer = new UpgradeUIRenderer()
            {
                BackgroundColor = Color.FromArgb(23, 23, 23),
                Size = new Size(500, 280)
            }
        };
        public static void Run()
        {
            Application.Run(ui);
        }
        internal static void SetUIRenderer(UpgradeUIRenderer upgradeUIRenderer)
        {
            if (ui.Visible)
            {
                ui.UIRenderer = upgradeUIRenderer;
            }
            else
            {
                Task.Run(async () =>
                {
                    while (!ui.Visible)
                    {
                        await Task.Delay(100);
                        SetUIRenderer(upgradeUIRenderer);
                    }
                });
            }
        }
        /// <summary>
        /// 设置将要显示的消息字符
        /// </summary>
        /// <param name="msg"></param>
        internal static void SetMessage(string msg)
        {
            ui.SetMsg(msg);
        }
        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="msg"></param>
        internal static void SetProgress(float progress)
        {
            ui.SetProgress(progress);
        }
        /// <summary>
        /// 设置升级内容
        /// </summary>
        /// <param name="msg"></param>
        internal static void SetUpgradeContent(string upgradeContent)
        {
            ui.SetUpgradeContent(upgradeContent);
        }
        /// <summary>
        /// 设置服务器版本
        /// </summary>
        /// <param name="msg"></param>
        internal static void SetVersionServer(string versionServer)
        {
            ui.SetVersionServer(versionServer);
        }
    }
}
