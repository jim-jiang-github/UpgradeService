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
using Newtonsoft.Json;
using FileTransfe;

namespace TransfeTest
{
    public partial class Form1 : Form
    {
        private ClientUpload clientUpload = new ClientUpload("http://localhost:22437/api/UpgradeServer/upload", "http://localhost:22437/api/UpgradeServer/merge");
        private ClientDownload clientDownload = new ClientDownload("http://localhost:22437/api/UpgradeServer/download");
        public Form1()
        {
            InitializeComponent();
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            string fileName = "donetRepair4.5.exe";
            clientDownload.DownloadProgressChanged += (s, ee) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.label1.Text = ee.ToString();
                }));
            };
            clientDownload.DownloadSpeedChanged += (s, ee) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.label2.Text = ee.ToString();
                }));
            };
            clientDownload.DownloadError += (s, ee) =>
            {
            };
            clientDownload.DownloadCompleted += (s, ee, ss) =>
            {
            };
            clientDownload.Download(fileName);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string fileName = "donetRepair4.5.exe";
            string downloadPartPath = Path.Combine(fileName + ".downloadPart");

            clientDownload.DownloadProgressChanged += (s, ee) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.label1.Text = ee.ToString();
                }));
            };
            clientDownload.DownloadSpeedChanged += (s, ee) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.label2.Text = ee.ToString();
                }));
            };
            clientDownload.DownloadError += (s, ee) =>
            {
            };
            clientDownload.DownloadCompleted += (s, ee, ss) =>
            {
            };
            clientDownload.ResumeDownload(downloadPartPath);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string filePath = @"E:\donetRepair4.5.exe";
            //string filePath = @"c:\1.jpg";
            //string filePath = @"E:/Symantec Ghost.rar";
            this.clientUpload.UploadProgressChanged += (s, p) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.label1.Text = p.ToString();
                }));
            };
            this.clientUpload.UploadSpeedChanged += (s, p) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.label2.Text = p.ToString();
                }));
            };
            this.clientUpload.UploadAsync(filePath);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //string filePath1 = @"C:\1.jpg";
            //using (WebClient webClient = new WebClient())
            //{
            //    webClient.UploadProgressChanged += (s, ee) =>
            //    {

            //    };
            //    webClient.UploadFileCompleted += (s, ee) =>
            //    {
            //        var asd = JsonConvert.DeserializeObject<RespondResult>(Encoding.UTF8.GetString(ee.Result));
            //    };
            //    webClient.UploadFileAsync(new Uri("http://localhost:22437/api/UpgradeServer/upload"), filePath1);
            //}
            //return;
            string filePath = @"E:\donetRepair4.5.exe";
            int chunkSize = 1024 * 1024 * 3; //3M缓存
            FileInfo fileInfo = new FileInfo(filePath);
            using (FileStream readStream = File.OpenRead(filePath))
            {
                for (long i = 0; i < fileInfo.Length; i += chunkSize)
                {
                    readStream.Position = i;
                    string chunkPath = Path.Combine("Merge", "chunk");
                    using (Stream writeStream = File.OpenWrite(chunkPath))
                    {
                        byte[] buffer = new byte[chunkSize];
                        int readLength = readStream.Read(buffer, 0, buffer.Length);
                        writeStream.Write(buffer, 0, readLength);
                    }
                    //byte[] chunkMD5 = MD5Tools.GetFileMd5Bytes(chunkPath); 
                    using (Stream read = File.OpenRead(chunkPath))
                    {
                        using (HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false }))//若想手动设置Cookie则必须设置UseCookies = false
                        {
                            MultipartFormDataContent content = new MultipartFormDataContent();
                            //content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                            //content.Headers.ContentMD5 = chunkMD5;
                            //content.Headers.ContentLength = readLength;
                            content.Add(new StreamContent(read, chunkSize), "aaa");
                            try
                            {
                                var result = client.PostAsync("http://localhost:22437/api/UpgradeServer/upload", content).Result;
                                if (result.IsSuccessStatusCode)
                                {
                                    string rslt = result.Content.ReadAsStringAsync().Result;
                                }
                                else
                                {

                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    File.Delete(chunkPath);
                }
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            using (HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) })
            {
                var result = httpClient.GetAsync("http://localhost:22437/api/UpgradeServer/merge?fileName=donetRepair4.5.exe").Result;
                if (result.IsSuccessStatusCode)
                {
                    string rslt = result.Content.ReadAsStringAsync().Result;
                }
                else
                {

                }
            }
        }
    }
}
