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
using System.Net;

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
            url = "http://120.79.81.121/api/UpgradeService/download?fileName=1.zip";
            url = $"https://ffmpeg.zeranoe.com/builds/win64/static/ffmpeg-latest-win64-static.zip";
            url = "https://dldir1.qq.com/qqfile/qq/PCQQ9.1.3/25323/QQ9.1.3.25323.exe";
            DownloadMsg downloadMsg = new DownloadMsg();
            downloadMsg.DownloadMsgChanged += (s, ee) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.label1.Text = downloadMsg.Speed.ToString();
                    this.label2.Text = downloadMsg.Progress.ToString();
                }));
                if (downloadMsg.Complete)
                {

                }
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

        private void Button3_Click(object sender, EventArgs e)
        {
            string fullPath = @"C:\1.jpg";
            string fullPath1 = @"C:\2.jpg";
            using (HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false }))//若想手动设置Cookie则必须设置UseCookies = false
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                #region Stream请求
                using (Stream stream = File.Open(fullPath, FileMode.Open, FileAccess.Read))
                using (Stream stream1 = File.Open(fullPath1, FileMode.Open, FileAccess.Read))
                {
                    content.Add(new StreamContent(stream, 1024 * 8), "file", Path.GetFileName(fullPath));
                    content.Add(new StreamContent(stream1, 1024 * 8), "file", Path.GetFileName(fullPath1));
                    var result = client.PostAsync("http://localhost:22437/api/UpgradeServer/upload", content).Result;

                    if (result.IsSuccessStatusCode)
                    {
                        string rslt = result.Content.ReadAsStringAsync().Result;


                    }
                }
                #endregion
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            string filePath = @"E:\donetRepair4.5.exe";
            int chunkSize = 1024 * 1024 * 3; //3M缓存
            FileInfo fileInfo = new FileInfo(filePath);
            using (FileStream readStream = File.OpenRead(filePath))
            {
                for (long i = 0; i < fileInfo.Length; i += chunkSize)
                {
                    readStream.Position = i;
                    using (Stream writeStream = new FileStream(Path.Combine("Merge", (i / chunkSize).ToString()), FileMode.Create, FileAccess.ReadWrite))
                    {
                        byte[] buffer = new byte[chunkSize];
                        int readLength = readStream.Read(buffer, 0, buffer.Length);
                        writeStream.Write(buffer, 0, readLength);
                        using (HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false }))//若想手动设置Cookie则必须设置UseCookies = false
                        {
                            MultipartFormDataContent content = new MultipartFormDataContent();
                            content.Add(new StreamContent(writeStream, chunkSize), "chunk_" + i, Path.GetFileName(filePath));
                            var result = client.PostAsync("http://localhost:22437/api/UpgradeServer/upload", content).Result;
                            if (result.IsSuccessStatusCode)
                            {
                                string rslt = result.Content.ReadAsStringAsync().Result;
                            }
                        }
                    }
                }
            }
            //using (HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false }))//若想手动设置Cookie则必须设置UseCookies = false
            //{
            //    MultipartFormDataContent content = new MultipartFormDataContent();
            //    for (long i = 0; i < fileInfo.Length; i += chunkSize)
            //    {
            //        using (PartialFileStream partialFileStream = new PartialFileStream(filePath, i, i + chunkSize))
            //        using (Stream writeStream = new FileStream(Path.Combine("Merge", (i / chunkSize).ToString()), FileMode.OpenOrCreate, FileAccess.ReadWrite))
            //        {
            //            partialFileStream.CopyTo(writeStream);
            //            content.Add(new StreamContent(partialFileStream, 1024 * 8), "file", Path.GetFileName(filePath));

            //            var result = client.PostAsync("http://localhost:22437/api/UpgradeServer/upload", content).Result;

            //            if (result.IsSuccessStatusCode)
            //            {
            //                string rslt = result.Content.ReadAsStringAsync().Result;


            //            }
            //        }
            //    }
            //}
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles("Merge").OrderBy(p => Convert.ToInt32(Path.GetFileNameWithoutExtension(p))).ToArray();
            using (Stream writeStream = File.OpenWrite(Path.Combine("Merge", "donetRepair4.511.exe")))
            {
                //long position = 0;
                for (int i = 0; i < files.Length; i++)
                {
                    FileStream chunk = File.OpenRead(files[i]);
                    //writeStream.Position = position;
                    byte[] buffer = new byte[1024];
                    int readLength = chunk.Read(buffer, 0, buffer.Length);
                    while (readLength > 0)
                    {
                        writeStream.Write(buffer, 0, readLength);
                        readLength = chunk.Read(buffer, 0, buffer.Length);
                    }
                    //chunk.CopyTo(writeStream);
                    //position += chunk.Length;
                }
            }
        }
    }
}
