namespace Upgrade.Program.Release
{
    partial class FmRelease
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRelease = new System.Windows.Forms.Button();
            this.btnProgramDirectory = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lsvVersion = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cbxFilterContain = new Upgrade.Program.Release.ConfigBoolProperty();
            this.lvFiles = new System.Windows.Forms.ListView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cbxDifferenceOnly = new Upgrade.Program.Release.ConfigBoolProperty();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tbxUpdateContent = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxExcludeFile = new Upgrade.Program.Release.ConfigTextProperty();
            this.tbxExcludeDir = new Upgrade.Program.Release.ConfigTextProperty();
            this.tbxExcludeExt = new Upgrade.Program.Release.ConfigTextProperty();
            this.tbxIncludeFile = new Upgrade.Program.Release.ConfigTextProperty();
            this.tbxMainExe = new Upgrade.Program.Release.ConfigTextProperty();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tbxUpgradeLocalVersion = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbxUpgradeServerVersion = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnUpdateUpgradeProgram = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tbxUpgradeExe = new Upgrade.Program.Release.ConfigTextProperty();
            this.rdbSelectUpdate = new System.Windows.Forms.RadioButton();
            this.rdbForceUpdate = new System.Windows.Forms.RadioButton();
            this.lvVersion = new System.Windows.Forms.Label();
            this.btnOpenDirectory = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tspbProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tslbProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbxProgramDirectory = new Upgrade.Program.Release.ConfigTextProperty();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRelease
            // 
            this.btnRelease.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRelease.Location = new System.Drawing.Point(836, 559);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(75, 23);
            this.btnRelease.TabIndex = 0;
            this.btnRelease.Text = "发布";
            this.btnRelease.UseVisualStyleBackColor = true;
            this.btnRelease.Click += new System.EventHandler(this.BtnRelease_Click);
            // 
            // btnProgramDirectory
            // 
            this.btnProgramDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProgramDirectory.Location = new System.Drawing.Point(759, 10);
            this.btnProgramDirectory.Name = "btnProgramDirectory";
            this.btnProgramDirectory.Size = new System.Drawing.Size(75, 23);
            this.btnProgramDirectory.TabIndex = 1;
            this.btnProgramDirectory.Text = "选择目录";
            this.btnProgramDirectory.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.lsvVersion);
            this.groupBox1.Location = new System.Drawing.Point(0, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 540);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "版本列表";
            // 
            // lsvVersion
            // 
            this.lsvVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvVersion.Location = new System.Drawing.Point(3, 17);
            this.lsvVersion.Name = "lsvVersion";
            this.lsvVersion.Size = new System.Drawing.Size(214, 520);
            this.lsvVersion.TabIndex = 6;
            this.lsvVersion.UseCompatibleStateImageBehavior = false;
            this.lsvVersion.View = System.Windows.Forms.View.List;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tabControl1);
            this.groupBox2.Location = new System.Drawing.Point(216, 39);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(695, 518);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "更新配置";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(689, 498);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbxFilterContain);
            this.tabPage1.Controls.Add(this.lvFiles);
            this.tabPage1.Controls.Add(this.btnRefresh);
            this.tabPage1.Controls.Add(this.cbxDifferenceOnly);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(681, 472);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "更新列表";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cbxFilterContain
            // 
            this.cbxFilterContain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxFilterContain.AutoSize = true;
            this.cbxFilterContain.Config = null;
            this.cbxFilterContain.Location = new System.Drawing.Point(435, 450);
            this.cbxFilterContain.Name = "cbxFilterContain";
            this.cbxFilterContain.PropertyName = null;
            this.cbxFilterContain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbxFilterContain.Size = new System.Drawing.Size(72, 16);
            this.cbxFilterContain.TabIndex = 10;
            this.cbxFilterContain.Text = "显示过滤";
            this.cbxFilterContain.UseVisualStyleBackColor = true;
            // 
            // lvFiles
            // 
            this.lvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFiles.Location = new System.Drawing.Point(3, 3);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(675, 441);
            this.lvFiles.TabIndex = 5;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.List;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(603, 446);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // cbxDifferenceOnly
            // 
            this.cbxDifferenceOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxDifferenceOnly.AutoSize = true;
            this.cbxDifferenceOnly.Config = null;
            this.cbxDifferenceOnly.Location = new System.Drawing.Point(513, 450);
            this.cbxDifferenceOnly.Name = "cbxDifferenceOnly";
            this.cbxDifferenceOnly.PropertyName = null;
            this.cbxDifferenceOnly.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbxDifferenceOnly.Size = new System.Drawing.Size(84, 16);
            this.cbxDifferenceOnly.TabIndex = 8;
            this.cbxDifferenceOnly.Text = "只显示不同";
            this.cbxDifferenceOnly.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tbxUpdateContent);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(681, 472);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "更新日志";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tbxUpdateContent
            // 
            this.tbxUpdateContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxUpdateContent.Location = new System.Drawing.Point(0, 0);
            this.tbxUpdateContent.Multiline = true;
            this.tbxUpdateContent.Name = "tbxUpdateContent";
            this.tbxUpdateContent.Size = new System.Drawing.Size(681, 472);
            this.tbxUpdateContent.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.tbxExcludeFile);
            this.tabPage2.Controls.Add(this.tbxExcludeDir);
            this.tabPage2.Controls.Add(this.tbxExcludeExt);
            this.tabPage2.Controls.Add(this.tbxIncludeFile);
            this.tabPage2.Controls.Add(this.tbxMainExe);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(681, 472);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "过滤设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "排除的文件";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "排除的目录";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "排除的后缀";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "主程序";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "包含的文件";
            // 
            // tbxExcludeFile
            // 
            this.tbxExcludeFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxExcludeFile.Config = null;
            this.tbxExcludeFile.Location = new System.Drawing.Point(71, 30);
            this.tbxExcludeFile.Name = "tbxExcludeFile";
            this.tbxExcludeFile.PropertyName = null;
            this.tbxExcludeFile.Size = new System.Drawing.Size(604, 21);
            this.tbxExcludeFile.TabIndex = 10;
            // 
            // tbxExcludeDir
            // 
            this.tbxExcludeDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxExcludeDir.Config = null;
            this.tbxExcludeDir.Location = new System.Drawing.Point(71, 57);
            this.tbxExcludeDir.Name = "tbxExcludeDir";
            this.tbxExcludeDir.PropertyName = null;
            this.tbxExcludeDir.Size = new System.Drawing.Size(604, 21);
            this.tbxExcludeDir.TabIndex = 8;
            // 
            // tbxExcludeExt
            // 
            this.tbxExcludeExt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxExcludeExt.Config = null;
            this.tbxExcludeExt.Location = new System.Drawing.Point(71, 3);
            this.tbxExcludeExt.Name = "tbxExcludeExt";
            this.tbxExcludeExt.PropertyName = null;
            this.tbxExcludeExt.Size = new System.Drawing.Size(604, 21);
            this.tbxExcludeExt.TabIndex = 0;
            // 
            // tbxIncludeFile
            // 
            this.tbxIncludeFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxIncludeFile.Config = null;
            this.tbxIncludeFile.Location = new System.Drawing.Point(71, 84);
            this.tbxIncludeFile.Name = "tbxIncludeFile";
            this.tbxIncludeFile.PropertyName = null;
            this.tbxIncludeFile.Size = new System.Drawing.Size(604, 21);
            this.tbxIncludeFile.TabIndex = 2;
            // 
            // tbxMainExe
            // 
            this.tbxMainExe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxMainExe.Config = null;
            this.tbxMainExe.Location = new System.Drawing.Point(71, 111);
            this.tbxMainExe.Name = "tbxMainExe";
            this.tbxMainExe.PropertyName = null;
            this.tbxMainExe.Size = new System.Drawing.Size(604, 21);
            this.tbxMainExe.TabIndex = 6;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tbxUpgradeLocalVersion);
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Controls.Add(this.tbxUpgradeServerVersion);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.btnUpdateUpgradeProgram);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.tbxUpgradeExe);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(681, 472);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "升级程序";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tbxUpgradeLocalVersion
            // 
            this.tbxUpgradeLocalVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxUpgradeLocalVersion.Location = new System.Drawing.Point(455, 31);
            this.tbxUpgradeLocalVersion.Name = "tbxUpgradeLocalVersion";
            this.tbxUpgradeLocalVersion.ReadOnly = true;
            this.tbxUpgradeLocalVersion.Size = new System.Drawing.Size(128, 21);
            this.tbxUpgradeLocalVersion.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(336, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 12);
            this.label8.TabIndex = 24;
            this.label8.Text = "升级程序本地版本：";
            // 
            // tbxUpgradeServerVersion
            // 
            this.tbxUpgradeServerVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxUpgradeServerVersion.Location = new System.Drawing.Point(202, 31);
            this.tbxUpgradeServerVersion.Name = "tbxUpgradeServerVersion";
            this.tbxUpgradeServerVersion.ReadOnly = true;
            this.tbxUpgradeServerVersion.Size = new System.Drawing.Size(128, 21);
            this.tbxUpgradeServerVersion.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(71, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 12);
            this.label7.TabIndex = 22;
            this.label7.Text = "升级程序服务器版本：";
            // 
            // btnUpdateUpgradeProgram
            // 
            this.btnUpdateUpgradeProgram.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateUpgradeProgram.Location = new System.Drawing.Point(586, 30);
            this.btnUpdateUpgradeProgram.Name = "btnUpdateUpgradeProgram";
            this.btnUpdateUpgradeProgram.Size = new System.Drawing.Size(90, 23);
            this.btnUpdateUpgradeProgram.TabIndex = 21;
            this.btnUpdateUpgradeProgram.Text = "更新升级程序";
            this.btnUpdateUpgradeProgram.UseVisualStyleBackColor = true;
            this.btnUpdateUpgradeProgram.Click += new System.EventHandler(this.BtnUpdateUpgradeProgram_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "升级程序";
            // 
            // tbxUpgradeExe
            // 
            this.tbxUpgradeExe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxUpgradeExe.Config = null;
            this.tbxUpgradeExe.Location = new System.Drawing.Point(72, 3);
            this.tbxUpgradeExe.Name = "tbxUpgradeExe";
            this.tbxUpgradeExe.PropertyName = null;
            this.tbxUpgradeExe.Size = new System.Drawing.Size(604, 21);
            this.tbxUpgradeExe.TabIndex = 19;
            // 
            // rdbSelectUpdate
            // 
            this.rdbSelectUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbSelectUpdate.AutoSize = true;
            this.rdbSelectUpdate.Location = new System.Drawing.Point(682, 563);
            this.rdbSelectUpdate.Name = "rdbSelectUpdate";
            this.rdbSelectUpdate.Size = new System.Drawing.Size(71, 16);
            this.rdbSelectUpdate.TabIndex = 5;
            this.rdbSelectUpdate.Text = "选择更新";
            this.rdbSelectUpdate.UseVisualStyleBackColor = true;
            // 
            // rdbForceUpdate
            // 
            this.rdbForceUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbForceUpdate.AutoSize = true;
            this.rdbForceUpdate.Checked = true;
            this.rdbForceUpdate.Location = new System.Drawing.Point(759, 563);
            this.rdbForceUpdate.Name = "rdbForceUpdate";
            this.rdbForceUpdate.Size = new System.Drawing.Size(71, 16);
            this.rdbForceUpdate.TabIndex = 6;
            this.rdbForceUpdate.TabStop = true;
            this.rdbForceUpdate.Text = "强制更新";
            this.rdbForceUpdate.UseVisualStyleBackColor = true;
            // 
            // lvVersion
            // 
            this.lvVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lvVersion.AutoSize = true;
            this.lvVersion.Location = new System.Drawing.Point(224, 565);
            this.lvVersion.Name = "lvVersion";
            this.lvVersion.Size = new System.Drawing.Size(29, 12);
            this.lvVersion.TabIndex = 7;
            this.lvVersion.Text = "版本";
            // 
            // btnOpenDirectory
            // 
            this.btnOpenDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenDirectory.Location = new System.Drawing.Point(836, 10);
            this.btnOpenDirectory.Name = "btnOpenDirectory";
            this.btnOpenDirectory.Size = new System.Drawing.Size(75, 23);
            this.btnOpenDirectory.TabIndex = 9;
            this.btnOpenDirectory.Text = "打开目录";
            this.btnOpenDirectory.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspbProgress,
            this.tslbProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 585);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(911, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tspbProgress
            // 
            this.tspbProgress.Name = "tspbProgress";
            this.tspbProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // tslbProgress
            // 
            this.tslbProgress.Name = "tslbProgress";
            this.tslbProgress.Size = new System.Drawing.Size(32, 17);
            this.tslbProgress.Text = "就绪";
            // 
            // tbxProgramDirectory
            // 
            this.tbxProgramDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxProgramDirectory.Config = null;
            this.tbxProgramDirectory.Location = new System.Drawing.Point(0, 12);
            this.tbxProgramDirectory.Name = "tbxProgramDirectory";
            this.tbxProgramDirectory.PropertyName = null;
            this.tbxProgramDirectory.ReadOnly = true;
            this.tbxProgramDirectory.Size = new System.Drawing.Size(753, 21);
            this.tbxProgramDirectory.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 607);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnOpenDirectory);
            this.Controls.Add(this.lvVersion);
            this.Controls.Add(this.rdbForceUpdate);
            this.Controls.Add(this.rdbSelectUpdate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbxProgramDirectory);
            this.Controls.Add(this.btnProgramDirectory);
            this.Controls.Add(this.btnRelease);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRelease;
        private System.Windows.Forms.Button btnProgramDirectory;
        private ConfigTextProperty tbxProgramDirectory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private ConfigTextProperty tbxExcludeExt;
        private System.Windows.Forms.Label label2;
        private ConfigTextProperty tbxIncludeFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.RadioButton rdbSelectUpdate;
        private System.Windows.Forms.RadioButton rdbForceUpdate;
        private System.Windows.Forms.Label label3;
        private ConfigTextProperty tbxMainExe;
        private System.Windows.Forms.Label lvVersion;
        private ConfigBoolProperty cbxDifferenceOnly;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private ConfigTextProperty tbxExcludeFile;
        private System.Windows.Forms.Label label5;
        private ConfigTextProperty tbxExcludeDir;
        private System.Windows.Forms.Label label4;
        private ConfigBoolProperty cbxFilterContain;
        private System.Windows.Forms.Button btnOpenDirectory;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar tspbProgress;
        private System.Windows.Forms.ToolStripStatusLabel tslbProgress;
        private System.Windows.Forms.ListView lsvVersion;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox tbxUpdateContent;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox tbxUpgradeLocalVersion;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbxUpgradeServerVersion;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnUpdateUpgradeProgram;
        private System.Windows.Forms.Label label6;
        private ConfigTextProperty tbxUpgradeExe;
    }
}

