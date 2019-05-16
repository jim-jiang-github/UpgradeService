using FileTransfe.Entities;
using FileTransfe.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileTransfe.Core;

namespace TransfeTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            string fileName = "BlueStacksGPSetup.exe";
            //string fileName = "CentOS-7-x86_64-DVD-1810.iso";
            //string fileName = "svn客户端设置.doc";
            //string url = "http://120.79.81.121:9099/api/Upgrade/download?fileName=" + fileName;
            string url = "http://localhost:9099/api/UpgradeService/download?fileName=" + fileName;
            url = "http://localhost:22437//api/UpgradeService/download?fileName=1.txt";
            //url = "http://127.0.0.1:9099/api/UpgradeService/download?fileName=1.txt";
            DownloadMsg downloadMsg = new DownloadMsg();
            downloadMsg.DownloadMsgChanged += (s, ee) => 
            {
                this.Invoke(new MethodInvoker(()=>
                {
                    this.label1.Text = downloadMsg.Speed.ToString();
                    this.label2.Text = downloadMsg.Progress.ToString();
                }));
            };
            ClientDownload.Download(new Uri(url), fileName, downloadMsg);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string fileName = "BlueStacksGPSetup.exe";
            string downloadPartPath = Path.Combine(fileName + ".downloadPart");

            DownloadMsg downloadMsg = new DownloadMsg();
            downloadMsg.DownloadMsgChanged += (s, ee) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.label1.Text = downloadMsg.Speed.ToString();
                    this.label2.Text = downloadMsg.Progress.ToString();
                }));
            };
            ClientDownload.Download(downloadPartPath, downloadMsg);
        }
    }
}
