using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private async void Button1_Click(object sender, EventArgs e)
        {
            string version = "2.0.7089.20904";
            if (await UpgradeOperation.Upgradeable(version)
                   && await UpgradeOperation.StartUpgradeClientWithConnect()
                   && await UpgradeOperation.UpgradeUI()
                   && await UpgradeOperation.CheckVersion(version))
            {
                Application.Exit();
            }
        }
        //protected async override void OnClosing(CancelEventArgs e)
        //{
        //    e.Cancel = true;
        //    await Task.Yield();
        //    if (await UpgradeOperation.CloseUpgradeClient())
        //    {
        //        Application.Exit();
        //    }
        //}

        private async void Button2_Click(object sender, EventArgs e)
        {
            string path = @"c:/1.jpg";
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(3000);
            if (File.Exists(path))
            {
                bool b = await Task<bool>.Run(() =>
                {
                    while (File.Exists(path) && !cancellationTokenSource.IsCancellationRequested)
                    {
                        Task.Delay(100);
                    }
                    return !cancellationTokenSource.IsCancellationRequested;
                }, cancellationTokenSource.Token);
            }
        }
    }
}
