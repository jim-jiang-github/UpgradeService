using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgrade.ProgramRelease
{
    public partial class Form1 : Form
    {
        private Config config = Config.Load();
        /// <summary>
        /// 当前文件扫描
        /// </summary>
        private FileScan[] fileScans = null;
        /// <summary>
        /// 上一次文件扫描
        /// </summary>
        private FileScan[] lastfileScans = UpgradeInfo.Load().FileScans;
        /// <summary>
        /// 上一次版本号
        /// </summary>
        private string lastVersion;
        /// <summary>
        /// 当前版本号
        /// </summary>
        private string currentVersion;
        public Form1()
        {
            InitializeComponent();
            this.lvFiles.MouseDoubleClick += (s, e) =>
            {
                if (this.lvFiles.SelectedItems.Count == 0) { return; }
                Clipboard.SetText(this.lvFiles.SelectedItems[0].Text);
                MessageBox.Show("“" + this.lvFiles.SelectedItems[0].Text + "” 已经复制到剪切板");
            };
            this.cbxDifferenceOnly.CheckedChanged += (s, e) =>
            {
                this.RefreshfileScans();
            };
            this.SelectProgramDirectory();
            this.tbxExclude.Config = this.config;
            this.tbxExclude.PropertyName = nameof(this.config.Exclude);
            this.tbxInclude.Config = this.config;
            this.tbxInclude.PropertyName = nameof(this.config.Include);
            this.tbxMainExe.Config = this.config;
            this.tbxMainExe.PropertyName = nameof(this.config.MainExe);
            this.cbxDifferenceOnly.Config = this.config;
            this.cbxDifferenceOnly.PropertyName = nameof(this.config.DifferenceOnly);
        }

        private void BtnProgramDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = config.ProgramDirectory;
            folderBrowserDialog.Description = "请选择发布程序所在的目录";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                config.ProgramDirectory = folderBrowserDialog.SelectedPath;
                config.Save();
                this.SelectProgramDirectory();
            }
        }
        /// <summary> 选择程序目录
        /// </summary>
        private void SelectProgramDirectory()
        {
            this.tbxProgramDirectory.Text = config.ProgramDirectory;
            if (!Directory.Exists(config.ProgramDirectory))
            {
                return;
            }
            this.RefreshfileScans();
        }
        private void RefreshfileScans()
        {
            this.fileScans = Directory.GetFiles(config.ProgramDirectory, "*", SearchOption.AllDirectories)
                .Where(p => !(config.Exclude ?? new string[0]).Contains(Path.GetExtension(p)) || (config.Include ?? new string[0]).Contains(Path.GetFileName(p)))
                //.Select(p => p.Replace(config.ProgramDirectory + "\\", ""))
                .AsParallel()
                .AsOrdered()
                .Select(p => new FileScan(p))
                .ToArray();
            FileScan[] deleteFileScans = this.lastfileScans.Where(f => this.fileScans.FirstOrDefault(fs => fs.Name == f.Name) == null).ToArray();
            foreach (FileScan fs in deleteFileScans)
            {
                fs.Result = FileScan.ScanResult.Remove;
            }
            FileScan[] results = deleteFileScans;
            results = results.Concat(this.fileScans.Select(f =>
            {
                FileScan equal = this.lastfileScans?.FirstOrDefault(l => l.Name == f.Name);
                f.Result = equal == null ? FileScan.ScanResult.Add : (equal.Version == f.Version ? FileScan.ScanResult.Normal : FileScan.ScanResult.Update);
                return f;
            })).ToArray();
            if (this.cbxDifferenceOnly.Checked)
            {
                results = results.Where(r => r.Result != FileScan.ScanResult.Normal).ToArray();
            }
            this.lvFiles.Items.Clear();
            this.lvFiles.Items.AddRange(results.Select(r =>
            {
                Color color = Color.Transparent;
                switch (r.Result)
                {
                    case FileScan.ScanResult.Add:
                        color = Color.LightGreen;
                        break;
                    case FileScan.ScanResult.Normal:
                        color = Color.Transparent;
                        break;
                    case FileScan.ScanResult.Remove:
                        color = Color.PaleVioletRed;
                        break;
                    case FileScan.ScanResult.Update:
                        color = Color.Yellow;
                        break;
                }
                return new ListViewItem(r.Name)
                {
                    BackColor = color
                };
            }).ToArray());
            if (this.config?.MainExe != null)
            {
                this.lastVersion = this.lastfileScans?.FirstOrDefault(f => f.Name == this.config.MainExe)?.Version;
                this.currentVersion = this.fileScans?.FirstOrDefault(f => f.Name == this.config.MainExe)?.Version;
                this.lvVersion.Text = "版本号：" + (lastVersion ?? " 未知 ") + " -----> " + (currentVersion ?? " 未知 ");
            }
            else
            {
                this.lvVersion.Text = string.Empty;
            }
        }

        private void BtnRelease_Click(object sender, EventArgs e)
        {
            if (this.Check())
            {
                UpgradeInfo upgradeInfo = new UpgradeInfo();
                upgradeInfo.FileScans = this.fileScans;
                upgradeInfo.Save();
                MessageBox.Show("发布成功！当前版本号为：" + this.currentVersion);
                this.RefreshfileScans();
            }
        }
        private bool Check()
        {
            if (this.lastVersion == this.currentVersion)
            {
                MessageBox.Show("发布的版本号与上次相同！请重新编译主程序版本号！");
                return false;
            }
            return true;
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshfileScans();
        }
    }
}
