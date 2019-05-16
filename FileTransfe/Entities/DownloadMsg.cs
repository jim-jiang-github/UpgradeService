using System;
using System.Collections.Generic;
using System.Text;

namespace FileTransfe.Entities
{
    public class DownloadMsg : EventArgs
    {
        public event EventHandler DownloadMsgChanged;
        protected virtual void OnDownloadMsgChanged()
        {
            this.DownloadMsgChanged?.Invoke(this, EventArgs.Empty);
        }
        private bool complete = false;
        private float speed = 0;
        private float? progress = null;
        /// <summary>
        /// 是否下载完成
        /// </summary>
        public bool Complete
        {
            get => this.complete;
            set
            {
                this.complete = value;
                this.OnDownloadMsgChanged();
            }
        }
        /// <summary>
        /// 下载速度，单位kb/s
        /// </summary>
        public float Speed
        {
            get => this.speed;
            set
            {
                this.speed = value;
                this.OnDownloadMsgChanged();
            }
        }
        /// <summary> 下载进度0~1,为空时代表无法获取进度
        /// </summary>
        public float? Progress
        {
            get => this.progress;
            set
            {
                this.progress = value;
                this.OnDownloadMsgChanged();
            }
        }
    }
}
