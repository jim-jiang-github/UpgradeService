using Share.Pipe;
using Share.Upgrade.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgrade.Program.Client
{
    public partial class Form1 : Form
    {
        private string upgradeClientExe = Path.GetFileName(Application.ExecutablePath);
        public Form1()
        {
            InitializeComponent();
        }
        protected async override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            JsonReleaseVersion jsonReleaseVersion = await Commands.CommandUrl.CheckVersion("2.0.7089.20904");
            if (jsonReleaseVersion != null)
            {
                if (await Commands.CommandUrl.Upgrade(jsonReleaseVersion))
                {
                    MessageBox.Show("完成");
                }
            }
        }
    }
}
