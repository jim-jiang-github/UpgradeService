/******************************************************************
* 使本项目源码或本项目生成的DLL前请仔细阅读以下协议内容，如果你同意以下协议才能使用本项目所有的功能，
* * 否则如果你违反了以下协议，有可能陷入法律纠纷和赔偿，作者保留追究法律责任的权利。
* * 1、你可以在开发的软件产品中使用和修改本项目的源码和DLL，但是请保留所有相关的版权信息。
* * 2、不能将本项目源码与作者的其他项目整合作为一个单独的软件售卖给他人使用。
* * 3、不能传播本项目的源码和DLL，包括上传到网上、拷贝给他人等方式。
* * 4、以上协议暂时定制，由于还不完善，作者保留以后修改协议的权利。
* 
*         Copyright (C):       煎饼的归宿
*         CLR版本:             4.0.30319.42000
*         注册组织名:          Microsoft
*         命名空间名称:        Initialize
*         文件名:              FmInitialize
*         当前系统时间:        2019/2/13 星期三 上午 9:44:33
*         当前登录用户名:      Administrator
*         创建年份:            2019
*         版权所有：           煎饼的归宿QQ：375324644
******************************************************************/
using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Upgrade.Program.Client
{
    /// <summary> 升级窗体
    /// </summary>
    internal class FmUpgrade : LayeredForm
    {
        #region 变量
        private SynchronizationContext synchronizationContext = null;
        private AnimateImage animateImage = null;
        private Font versionFont = new Font("黑体", 11);
        private Font copyrightFont = new Font("黑体", 8);
        private Font companyFont = new Font("黑体", 9);
        private float progress = 0;
        private string upgradeContent = string.Empty;
        private string versionServer = string.Empty;
        private UpgradeUIRenderer renderer = null;
        private LayeredButton dUIButton = new LayeredButton() { Texture = global::Upgrade.Program.Client.Properties.Resources.buttonClose };
        #endregion
        #region 属性
        internal UpgradeUIRenderer UIRenderer
        {
            get => renderer;
            set
            {
                renderer = value;
                if (renderer != null)
                {
                    if (renderer.BackgroundImage != null)
                    {
                        this.SetImageUI(renderer.BackgroundImage);
                    }
                    else
                    {
                        this.SetDefaultUI(renderer.Size, renderer.BackgroundColor);
                    }
                }
            }
        }
        #endregion
        internal FmUpgrade()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.synchronizationContext = SynchronizationContext.Current;
            this.Font = new Font("黑体", 10);
            this.Icon = global::Upgrade.Program.Client.Properties.Resources.logo;
            //this.ShowInTaskbar = false;
            WindowsTools.SetTopomost(this.Handle);
            this.dUIButton.ControlParent = this;
            this.dUIButton.MouseClick += (s, e) => 
            {
                this.Close();
            };
        }
        #region 重写
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.TopMost = true;
            this.Activate();
            this.TopMost = false;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            this.animateImage?.Dispose();
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.dUIButton.X = this.Width - this.dUIButton.Width;
        }
        #endregion
        #region 函数
        /// <summary> 设置显示的消息
        /// </summary>
        /// <param name="msg"></param>
        internal void SetMsg(string msg)
        {
            if (!this.IsDisposed)
            {
                this.synchronizationContext.Send((obj) =>
                {
                    this.Text = msg;
                    this.Invalidate();
                }, null);
            }
        }
        /// <summary> 设置进度条
        /// </summary>
        /// <param name="progress"></param>
        internal void SetProgress(float progress)
        {
            if (!this.IsDisposed)
            {
                this.synchronizationContext.Send((obj) =>
                {
                    this.progress = progress;
                    this.Invalidate();
                }, null);
            }
        }
        /// <summary> 设置默认属性
        /// </summary>
        /// <param name="size"></param>
        private void SetDefaultUI(Size size, Color backgroundColor)
        {
            Bitmap backgroundImage = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(backgroundImage))
            {
                g.Clear(backgroundColor);
            }
            SetImageUI(backgroundImage);
        }
        /// <summary> 设置启动图
        /// </summary>
        /// <param name="image"></param>
        private void SetImageUI(Image image)
        {
            this.SetUISize(new Size(image.Width, image.Height));
            this.animateImage = new AnimateImage(image);
            if (this.animateImage.CanAnimate)
            {
                this.animateImage.OnFrameChanged += (s, e) =>
                {
                    this.Invalidate();
                };
            }
            else
            {
                this.Invalidate();
            }
        }
        /// <summary> 设置窗体尺寸
        /// </summary>
        /// <param name="size"></param>
        private void SetUISize(Size size)
        {
            this.Width = size.Width;
            this.Height = size.Height;
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
        }
        /// <summary> 设置窗体尺寸
        /// </summary>
        /// <param name="size"></param>
        public void SetUpgradeContent(string upgradeContent)
        {
            this.upgradeContent = upgradeContent;
            this.Invalidate();
        }
        /// <summary> 设置服务器版本
        /// </summary>
        /// <param name="size"></param>
        public void SetVersionServer(string versionServer)
        {
            this.versionServer = versionServer;
            this.Invalidate();
        }
        protected override void NotifyInvalidate(Rectangle invalidatedArea)
        {
            base.NotifyInvalidate(invalidatedArea);
            this.OnPaint(null);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.renderer != null)
            {
                using (Bitmap bmp = new Bitmap(this.Width, this.Height))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    this.animateImage?.Draw(g);
                    float border = 5;
                    float progressHeight = 8;
                    string copyright = this.renderer.Copyright ?? " ";
                    string version = this.renderer.Version ?? " ";
                    string company = this.renderer.Company ?? " ";
                    SizeF copyrightSize = g.MeasureString(copyright, this.copyrightFont);
                    SizeF versionSize = g.MeasureString(version, this.versionFont);
                    SizeF versionServerSize = g.MeasureString(this.versionServer, this.versionFont);
                    SizeF companySize = g.MeasureString(company, this.companyFont);
                    g.DrawString(company, this.companyFont, Brushes.White, new PointF(this.Width - border - companySize.Width, this.Height - companySize.Height - copyrightSize.Height - border - progressHeight));
                    g.DrawString(copyright, this.copyrightFont, Brushes.White, new PointF(this.Width - border - copyrightSize.Width, this.Height - copyrightSize.Height - border - progressHeight));
                    g.DrawString("v" + version, this.versionFont, Brushes.White, new PointF(border, border));
                    g.DrawString((string.IsNullOrEmpty(this.versionServer) ? string.Empty : " -----> v") + this.versionServer, this.versionFont, Brushes.White, new PointF(border + versionSize.Width, border));

                    g.DrawString(this.Text, this.Font, Brushes.White, new PointF(border, this.Height - companySize.Height - border - progressHeight));

                    g.FillRectangle(Brushes.Gray, new RectangleF(0, this.Height - progressHeight, this.Width, progressHeight));
                    g.FillRectangle(Brushes.SteelBlue, new RectangleF(0, this.Height - progressHeight, this.Width * this.progress, progressHeight));

                    RectangleF upgradeContent = new RectangleF(border + 180, border + versionSize.Height + border, this.Width - (border + 180) - border, 218 - (border + versionSize.Height + border));
                    g.SetClip(upgradeContent);
                    using (SolidBrush brush = new SolidBrush(this.renderer.BackgroundColor))
                    {
                        //g.FillRectangle(brush, upgradeContent);
                    }
                    g.DrawString(this.upgradeContent + this.upgradeContent + this.upgradeContent + this.upgradeContent, this.Font, Brushes.White, new RectangleF(upgradeContent.X, upgradeContent.Y - scrollY, upgradeContent.Width, upgradeContent.Height + scrollY));

                    base.OnPaint(new PaintEventArgs(g, new Rectangle(0, 0, this.Width, this.Height)));
                    this.SetBits((Bitmap)bmp);
                }
            }
        }
        private float scrollY = 0;
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
            {
                scrollY -= 10;
            }
            else
            {
                scrollY += 10;
            }
            if (scrollY < 0) { scrollY = 0; }
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                int border = 5;
                SizeF versionServerSize = g.MeasureString(this.upgradeContent, this.Font, this.Width - (border + 180) - border);
                if (scrollY > versionServerSize.Height + 218) { scrollY = versionServerSize.Height + 218; }
            }
            this.Invalidate();
        }
        #endregion
    }
}
