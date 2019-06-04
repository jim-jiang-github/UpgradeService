using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrade.Program.Client
{
    /// <summary> 初始化界面UI渲染器
    /// </summary>
    public class UpgradeUIRenderer
    {
        public virtual Image BackgroundImage { get; set; }
        public virtual Color BackgroundColor { get; set; } 
        public virtual Size Size { get; set; } 
        public virtual string Version { get; set; }
        public virtual string Copyright { get; set; }
        public virtual string Company { get; set; }
    }
}
