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
*         命名空间名称:        Common
*         文件名:              AnimateImage
*         当前系统时间:        2019/2/13 星期三 上午 10:04:57
*         当前登录用户名:      Administrator
*         创建年份:            2019
*         版权所有：           煎饼的归宿QQ：375324644
******************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Upgrade.Program.Client
{
    /// <summary>   
    /// 表示一类带动画功能的图像。   
    /// </summary>   
    public class AnimateImage : IDisposable
    {
        private Image image;
        private FrameDimension frameDimension = null;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        /// <summary>   
        /// 动画当前帧发生改变时触发。   
        /// </summary>   
        public event EventHandler<EventArgs> OnFrameChanged;
        /// <summary>   
        /// 实例化一个AnimateImage。   
        /// </summary>   
        /// <param name="img">动画图片。</param>   
        public AnimateImage(Image img)
        {
            image = img;
            lock (image)
            {
                this.CanAnimate = ImageAnimator.CanAnimate(image);
                if (this.CanAnimate)
                {
                    Guid[] guid = image.FrameDimensionsList;
                    this.frameDimension = new FrameDimension(guid[0]);
                    this.FrameCount = image.GetFrameCount(frameDimension);
                    Task.Factory.StartNew(() =>
                    {
                        while (!this.cancellationTokenSource.IsCancellationRequested)
                        {
                            this.CurrentFrame = this.CurrentFrame + 1 >= this.FrameCount ? 0 : this.CurrentFrame + 1;
                            lock (image)
                            {
                                image.SelectActiveFrame(frameDimension, this.CurrentFrame);
                            }
                            this.OnFrameChanged?.Invoke(this.image, EventArgs.Empty);
                            Thread.Sleep(30);
                        }
                    }, this.cancellationTokenSource.Token);
                }
            }
        }
        /// <summary> 图片
        /// </summary>   
        public Image Image
        {
            get { return image; }
        }
        /// <summary> 是否动画
        /// </summary>   
        public bool CanAnimate { get; private set; }
        /// <summary> 总帧数
        /// </summary>   
        public int FrameCount { get; private set; }
        /// <summary> 播放的当前帧
        /// </summary>   
        public int CurrentFrame { get; private set; }
        public void Draw(Graphics graphics)
        {
            if (cancellationTokenSource.IsCancellationRequested) { return; }
            lock (image)
            {
                graphics.DrawImage(image, 0, 0, image.Width, image.Height);
            }
        }
        public void Dispose()
        {
            this.cancellationTokenSource.Cancel();
            lock (image)
            {
                this.image.Dispose();
            }
        }
    }
}
