namespace Upgrade.ProgramRelease
{
    partial class Form1
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
            this.tbxProgramDirectory = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxUpdateContent = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbxDifferenceOnly = new Upgrade.ProgramRelease.ConfigBoolProperty();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxMainExe = new Upgrade.ProgramRelease.ConfigTextProperty();
            this.lvFiles = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxInclude = new Upgrade.ProgramRelease.ConfigTextProperty();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxExclude = new Upgrade.ProgramRelease.ConfigTextProperty();
            this.rdbSelectUpdate = new System.Windows.Forms.RadioButton();
            this.rdbForceUpdate = new System.Windows.Forms.RadioButton();
            this.lvVersion = new System.Windows.Forms.Label();
            this.rdbSilentUpdate = new System.Windows.Forms.RadioButton();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRelease
            // 
            this.btnRelease.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRelease.Location = new System.Drawing.Point(810, 486);
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
            this.btnProgramDirectory.Location = new System.Drawing.Point(810, 10);
            this.btnProgramDirectory.Name = "btnProgramDirectory";
            this.btnProgramDirectory.Size = new System.Drawing.Size(75, 23);
            this.btnProgramDirectory.TabIndex = 1;
            this.btnProgramDirectory.Text = "程序目录";
            this.btnProgramDirectory.UseVisualStyleBackColor = true;
            this.btnProgramDirectory.Click += new System.EventHandler(this.BtnProgramDirectory_Click);
            // 
            // tbxProgramDirectory
            // 
            this.tbxProgramDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxProgramDirectory.Location = new System.Drawing.Point(13, 12);
            this.tbxProgramDirectory.Name = "tbxProgramDirectory";
            this.tbxProgramDirectory.ReadOnly = true;
            this.tbxProgramDirectory.Size = new System.Drawing.Size(791, 21);
            this.tbxProgramDirectory.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.tbxUpdateContent);
            this.groupBox1.Location = new System.Drawing.Point(13, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 470);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "更新内容";
            // 
            // tbxUpdateContent
            // 
            this.tbxUpdateContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxUpdateContent.Location = new System.Drawing.Point(3, 17);
            this.tbxUpdateContent.Multiline = true;
            this.tbxUpdateContent.Name = "tbxUpdateContent";
            this.tbxUpdateContent.Size = new System.Drawing.Size(263, 450);
            this.tbxUpdateContent.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnRefresh);
            this.groupBox2.Controls.Add(this.cbxDifferenceOnly);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbxMainExe);
            this.groupBox2.Controls.Add(this.lvFiles);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tbxInclude);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tbxExclude);
            this.groupBox2.Location = new System.Drawing.Point(288, 39);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(597, 441);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "更新内容";
            // 
            // cbxDifferenceOnly
            // 
            this.cbxDifferenceOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxDifferenceOnly.AutoSize = true;
            this.cbxDifferenceOnly.Config = null;
            this.cbxDifferenceOnly.Location = new System.Drawing.Point(426, 416);
            this.cbxDifferenceOnly.Name = "cbxDifferenceOnly";
            this.cbxDifferenceOnly.PropertyName = null;
            this.cbxDifferenceOnly.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbxDifferenceOnly.Size = new System.Drawing.Size(84, 16);
            this.cbxDifferenceOnly.TabIndex = 8;
            this.cbxDifferenceOnly.Text = "只显示不同";
            this.cbxDifferenceOnly.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "主程序";
            // 
            // tbxMainExe
            // 
            this.tbxMainExe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxMainExe.Config = null;
            this.tbxMainExe.Location = new System.Drawing.Point(72, 71);
            this.tbxMainExe.Name = "tbxMainExe";
            this.tbxMainExe.PropertyName = null;
            this.tbxMainExe.Size = new System.Drawing.Size(519, 21);
            this.tbxMainExe.TabIndex = 6;
            // 
            // lvFiles
            // 
            this.lvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFiles.Location = new System.Drawing.Point(6, 98);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(585, 308);
            this.lvFiles.TabIndex = 5;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.List;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "包含的文件";
            // 
            // tbxInclude
            // 
            this.tbxInclude.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxInclude.Config = null;
            this.tbxInclude.Location = new System.Drawing.Point(72, 44);
            this.tbxInclude.Name = "tbxInclude";
            this.tbxInclude.PropertyName = null;
            this.tbxInclude.Size = new System.Drawing.Size(519, 21);
            this.tbxInclude.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "排除的后缀";
            // 
            // tbxExclude
            // 
            this.tbxExclude.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxExclude.Config = null;
            this.tbxExclude.Location = new System.Drawing.Point(72, 17);
            this.tbxExclude.Name = "tbxExclude";
            this.tbxExclude.PropertyName = null;
            this.tbxExclude.Size = new System.Drawing.Size(519, 21);
            this.tbxExclude.TabIndex = 0;
            // 
            // rdbSelectUpdate
            // 
            this.rdbSelectUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbSelectUpdate.AutoSize = true;
            this.rdbSelectUpdate.Location = new System.Drawing.Point(656, 490);
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
            this.rdbForceUpdate.Location = new System.Drawing.Point(733, 490);
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
            this.lvVersion.Location = new System.Drawing.Point(295, 492);
            this.lvVersion.Name = "lvVersion";
            this.lvVersion.Size = new System.Drawing.Size(0, 12);
            this.lvVersion.TabIndex = 7;
            // 
            // rdbSilentUpdate
            // 
            this.rdbSilentUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbSilentUpdate.AutoSize = true;
            this.rdbSilentUpdate.Location = new System.Drawing.Point(579, 490);
            this.rdbSilentUpdate.Name = "rdbSilentUpdate";
            this.rdbSilentUpdate.Size = new System.Drawing.Size(71, 16);
            this.rdbSilentUpdate.TabIndex = 8;
            this.rdbSilentUpdate.Text = "静默更新";
            this.rdbSilentUpdate.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(516, 412);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 521);
            this.Controls.Add(this.rdbSilentUpdate);
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
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRelease;
        private System.Windows.Forms.Button btnProgramDirectory;
        private System.Windows.Forms.TextBox tbxProgramDirectory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbxUpdateContent;
        private System.Windows.Forms.GroupBox groupBox2;
        private ConfigTextProperty tbxExclude;
        private System.Windows.Forms.Label label2;
        private ConfigTextProperty tbxInclude;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.RadioButton rdbSelectUpdate;
        private System.Windows.Forms.RadioButton rdbForceUpdate;
        private System.Windows.Forms.Label label3;
        private ConfigTextProperty tbxMainExe;
        private System.Windows.Forms.Label lvVersion;
        private ConfigBoolProperty cbxDifferenceOnly;
        private System.Windows.Forms.RadioButton rdbSilentUpdate;
        private System.Windows.Forms.Button btnRefresh;
    }
}

