using Share.Upgrade.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgrade.Program.Release
{
    public partial class FmRelease : Form
    {
        private string url = "http://localhost:9099/api/UpgradeServer";
        private Config config = Config.Load();
        private UpgradeClient upgradeClient = null;
        private JsonReleaseVersion[] versions = null;
        /// <summary>
        /// 当前文件扫描
        /// </summary>
        private FileScan[] fileScans = null;
        private bool callRefreshFileScans = true;
        /// <summary>
        /// 上一次版本号
        /// </summary>
        private Version lastVersion;
        /// <summary>
        /// 当前版本号
        /// </summary>
        private Version currentVersion;
        private JsonReleaseVersion ServerVersion => this.GetJsonReleaseVersion();
        private FileScan[] FileScans
        {
            get
            {
                if (this.fileScans == null || this.callRefreshFileScans)
                {
                    //是否被过滤
                    Func<string, bool> funcFilter = (f) =>
                    ((config.ExcludeExt ?? new string[0]).Contains(Path.GetExtension(f))
                    || (config.ExcludeFile ?? new string[0]).Contains(Path.GetFileName(f))
                    || (config.ExcludeDir ?? new string[0]).Contains(this.GetBaseDirectoryName(f, null)))
                    && !(config.IncludeFile ?? new string[0]).Contains(Path.GetFileName(f))
                    || (config.UpgradeExe ?? string.Empty) == Path.GetFileName(f);
                    //查找到的所有文件
                    IEnumerable<string> files = Directory.GetFiles(config.ProgramDirectory, "*", SearchOption.AllDirectories)
                        .Select(f => f.Replace(config.ProgramDirectory + "\\", ""));
                    //过滤后的文件
                    IEnumerable<string> filters = files.Where(f => !funcFilter(f));
                    //对比之前的文件信息计算出当前需要被删除的文件
                    var deletes = (this.ServerVersion?.Files?.Where(f => files.FirstOrDefault(fn => fn == f.Name) == null).ToArray() ?? new JsonFileDetail[0]).Select(d => new FileScan(d) { Result = FileScan.CompareResult.Remove });
                    this.fileScans = files
                        .AsParallel()
                        .AsOrdered()
                        .Select(p =>
                        {
                            bool isFilter = funcFilter.Invoke(p);
                            FileScan fileScan = new FileScan(config.ProgramDirectory, p, !isFilter);
                            if (isFilter)
                            {
                                fileScan.Result = FileScan.CompareResult.Filter;
                            }
                            else
                            {
                                JsonFileDetail equal = this.ServerVersion?.Files?.FirstOrDefault(l => l.Name == fileScan.Name);
                                fileScan.Result = equal == null ? FileScan.CompareResult.Add : (equal.MD5 == fileScan.MD5 ? FileScan.CompareResult.Normal : FileScan.CompareResult.Update);
                            }
                            return fileScan;
                        }).ToArray().Concat(deletes).ToArray();

                    this.callRefreshFileScans = false;
                }
                return this.fileScans;
            }
        }
        public FmRelease()
        {
            InitializeComponent();
            RateLimitor rateLimitor = new RateLimitor();
            this.upgradeClient = new UpgradeClient(config.ProgramDirectory, url);
            this.upgradeClient.UpgradeProgressChanged += (s, e) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    if (e.Total <= 0)
                    {
                        this.tspbProgress.Value = 0;
                    }
                    else
                    {
                        this.tspbProgress.Value = (int)((decimal)e.Loaded / e.Total * 100);
                    }
                    this.tspbProgress.Maximum = 100;
                    this.tslbProgress.Text = e.ToString();
                }));
            };
            this.upgradeClient.UpgradeCompleted += (s, e) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.tspbProgress.Value = 0;
                    this.tspbProgress.Maximum = 100;
                    this.tslbProgress.Text = "就绪";
                }));
            };
            this.lvFiles.MouseDoubleClick += (s, e) =>
            {
                if (this.lvFiles.SelectedItems.Count == 0) { return; }
                Clipboard.SetText(this.lvFiles.SelectedItems[0].Text);
                MessageBox.Show("“" + this.lvFiles.SelectedItems[0].Text + "” 已经复制到剪切板");
            };
            this.cbxDifferenceOnly.CheckedChanged += async (s, e) =>
             {
                 this.cbxDifferenceOnly.Enabled = false;
                 await this.RefreshfileScans();
                 this.cbxDifferenceOnly.Enabled = true;
             };
            this.cbxFilterContain.CheckedChanged += async (s, e) =>
            {
                this.cbxDifferenceOnly.Enabled = false;
                await this.RefreshfileScans();
                this.cbxDifferenceOnly.Enabled = true;
            };
            this.btnOpenDirectory.Click += (s, e) =>
            {
                Process.Start("explorer", this.config.ProgramDirectory);
            };
            this.btnRefresh.Click += async (s, e) =>
            {
                this.btnRefresh.Enabled = false;
                await this.RefreshfileScans();
                this.btnRefresh.Enabled = true;
            };
            this.btnProgramDirectory.Click += async (s, e) =>
            {
                this.btnProgramDirectory.Enabled = false;
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.SelectedPath = config.ProgramDirectory;
                folderBrowserDialog.Description = "请选择发布程序所在的目录";
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    config.ProgramDirectory = folderBrowserDialog.SelectedPath;
                    config.Save();
                    await this.SelectProgramDirectory();
                }
                this.btnProgramDirectory.Enabled = true;
            };
            this.tbxProgramDirectory.Config = this.config;
            this.tbxProgramDirectory.PropertyName = nameof(this.config.ProgramDirectory);
            this.tbxExcludeExt.Config = this.config;
            this.tbxExcludeExt.PropertyName = nameof(this.config.ExcludeExt);
            this.tbxExcludeFile.Config = this.config;
            this.tbxExcludeFile.PropertyName = nameof(this.config.ExcludeFile);
            this.tbxExcludeDir.Config = this.config;
            this.tbxExcludeDir.PropertyName = nameof(this.config.ExcludeDir);
            this.tbxIncludeFile.Config = this.config;
            this.tbxIncludeFile.PropertyName = nameof(this.config.IncludeFile);
            this.tbxMainExe.Config = this.config;
            this.tbxMainExe.PropertyName = nameof(this.config.MainExe);
            this.tbxUpgradeExe.Config = this.config;
            this.tbxUpgradeExe.PropertyName = nameof(this.config.UpgradeExe);
            this.tbxUpgradeExe.TextChanged += (s, e) =>
            {
                rateLimitor.Debounce(800, () =>
                {
                    this.Invoke(new MethodInvoker(async () =>
                    {
                        await this.RefreshVersion();
                    }));
                });
            };
            this.cbxDifferenceOnly.Config = this.config;
            this.cbxDifferenceOnly.PropertyName = nameof(this.config.DifferenceOnly);
            this.cbxFilterContain.Config = this.config;
            this.cbxFilterContain.PropertyName = nameof(this.config.FilterContain);
            this.Enabled = false;
        }
        private async Task RefreshVersion()
        {
            if (this.config.ProgramDirectory == null)
            {
                return;
            }
            string serverVersion = await this.upgradeClient.GetVersion("Upgrade.exe");
            if (serverVersion != null)
            {
                this.tbxUpgradeServerVersion.Text = serverVersion;
            }
            else
            {
                this.tbxUpgradeServerVersion.Text = string.Empty;
            }
            string upgradePath = Path.Combine(this.config.ProgramDirectory, this.tbxUpgradeExe.Text);
            if (File.Exists(upgradePath))
            {
                this.tbxUpgradeExe.BackColor = Color.GreenYellow;
                this.tbxUpgradeLocalVersion.Text = FileVersionInfo.GetVersionInfo(upgradePath).FileVersion;
                this.btnUpdateUpgradeProgram.Enabled = this.tbxUpgradeServerVersion.Text != this.tbxUpgradeLocalVersion.Text;
            }
            else
            {
                this.tbxUpgradeExe.BackColor = Color.OrangeRed;
                this.tbxUpgradeLocalVersion.Text = string.Empty;
                this.btnUpdateUpgradeProgram.Enabled = false;
            }
        }
        protected async override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await RefreshVersions();
            await this.SelectProgramDirectory();
            await this.RefreshVersion();
            this.Enabled = true;
        }
        /// <summary> 选择程序目录
        /// </summary>
        private async Task SelectProgramDirectory()
        {
            if (!Directory.Exists(config.ProgramDirectory))
            {
                return;
            }
            await this.RefreshfileScans();
        }
        /// <summary>
        /// 刷新版本列表
        /// </summary>
        /// <returns></returns>
        private async Task RefreshVersions()
        {
            this.versions = await this.upgradeClient.GetVersionList();
            if (this.versions != null)
            {
                this.lsvVersion.Items.Clear();
                foreach (JsonReleaseVersion jsonReleaseVersion in this.versions.Reverse())
                {
                    this.lsvVersion.Items.Add(jsonReleaseVersion.Version);
                }
            }
        }
        /// <summary>
        /// 刷新文件扫描列表
        /// </summary>
        /// <returns></returns>
        private async Task RefreshfileScans()
        {
            await Task<FileScan[]>.Run(() =>
            {
                this.callRefreshFileScans = true;
                return this.FileScans
                    .Where(f => this.cbxFilterContain.Checked ? true : f.Result != FileScan.CompareResult.Filter)
                    .Where(f => this.cbxDifferenceOnly.Checked ? f.Result != FileScan.CompareResult.Normal : f.Result == FileScan.CompareResult.Normal).ToArray();

            }).ContinueWith((fs) =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    var fss = fs.Result;
                    this.lvFiles.Items.Clear();
                    this.lvFiles.Items.AddRange(fss.Select(r =>
                    {
                        Color color = Color.Transparent;
                        switch (r.Result)
                        {
                            case FileScan.CompareResult.Filter:
                                color = Color.Gray;
                                break;
                            case FileScan.CompareResult.Add:
                                color = Color.LightGreen;
                                break;
                            case FileScan.CompareResult.Normal:
                                color = Color.Transparent;
                                break;
                            case FileScan.CompareResult.Remove:
                                color = Color.PaleVioletRed;
                                break;
                            case FileScan.CompareResult.Update:
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
                        this.lastVersion = new Version(this.ServerVersion?.Version ?? "0.0.0.0");
                        this.currentVersion = new Version(fss?.FirstOrDefault(f => f.Name == this.config.MainExe)?.Version ?? "0.0.0.0");
                        this.lvVersion.Text = "版本号：" + lastVersion.ToString() + " -----> " + currentVersion.ToString();
                    }
                    else
                    {
                        this.lvVersion.Text = string.Empty;
                    }
                }));
            });
        }
        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns></returns>
        private JsonReleaseVersion GetJsonReleaseVersion()
        {
            return (this.versions == null || this.versions.Length == 0) ? null : this.versions[this.versions.Length - 1];
        }
        /// <summary>
        /// 获取程序目录下的文件的基目录
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetBaseDirectoryName(string basePath, string path = null)
        {
            string baseDirectoryName = Path.GetDirectoryName(path ?? basePath);
            if (baseDirectoryName == string.Empty)
            {
                if (path == null)
                {
                    return string.Empty;
                }
                else
                {
                    return path;
                }
            }
            return GetBaseDirectoryName(basePath, baseDirectoryName);
        }
        /// <summary>
        /// 获取升级类型
        /// </summary>
        private JsonReleaseVersion.ReleaseType ReleaseType
        {
            get
            {
                if (this.rdbForceUpdate.Checked)
                {
                    return JsonReleaseVersion.ReleaseType.Force;
                }
                if (this.rdbSelectUpdate.Checked)
                {
                    return JsonReleaseVersion.ReleaseType.Choice;
                }
                return JsonReleaseVersion.ReleaseType.Force;
            }
        }
        private async void BtnRelease_Click(object sender, EventArgs e)
        {
            this.btnRelease.Enabled = false;
            if (this.Check())
            {
                JsonReleaseVersion jsonReleaseVersion = new JsonReleaseVersion()
                {
                    Version = this.currentVersion.ToString(),
                    Files = this.FileScans
                    .Where(f => f.Result == FileScan.CompareResult.Add || f.Result == FileScan.CompareResult.Update)
                    .Select(f => new JsonFileDetail() { Name = f.Name, MD5 = f.MD5, Length = f.Length, Version = this.currentVersion.ToString() })
                    .ToArray(),
                    Deletes = this.FileScans
                    .Where(f => f.Result == FileScan.CompareResult.Remove)
                    .Select(f => f.Name)
                    .ToArray(),
                    Type = this.ReleaseType,
                    UpdateContent = this.tbxUpdateContent.Text
                };
                if (await this.upgradeClient.Upgrade(jsonReleaseVersion))
                {
                    await this.RefreshVersions();
                    await this.RefreshfileScans();
                    MessageBox.Show("版本：" + jsonReleaseVersion.Version + "发布成功！");
                }
                else
                {
                    MessageBox.Show("发布失败！");
                }
            }
            this.btnRelease.Enabled = true;
        }
        private bool Check()
        {
            FileScan fileScan = this.FileScans
                .Where(f => f.Result == FileScan.CompareResult.Add || f.Result == FileScan.CompareResult.Update)
                .FirstOrDefault(f => f.Name == config.MainExe);
            if (this.lastVersion > this.currentVersion)
            {
                MessageBox.Show("发布的版本号低于服务器版本！请重新编译主程序版本号！");
                return false;
            }
            if (this.lastVersion == this.currentVersion)
            {
                MessageBox.Show("发布的版本号与上次相同！请重新编译主程序版本号！");
                return false;
            }
            return true;
        }

        private async void BtnUpdateUpgradeProgram_Click(object sender, EventArgs e)
        {
            string upgradePath = Path.Combine(this.config.ProgramDirectory, this.tbxUpgradeExe.Text);
            if (File.Exists(upgradePath))
            {
                if (await this.upgradeClient.UpdateUpgradePrograme(upgradePath))
                {
                    MessageBox.Show("升级程序：" + FileVersionInfo.GetVersionInfo(upgradePath).FileVersion + " 发布成功！");
                    await this.RefreshVersion();
                    return;
                }
            }
            MessageBox.Show("升级程序发布失败");
        }
    }
}
