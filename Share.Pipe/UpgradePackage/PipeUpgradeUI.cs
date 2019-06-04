using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Share.Pipe.UpgradePackage
{
    public class PipeUpgradeUI : Binary<PipeUpgradeUI>
    {
        public override byte Head => 7;
        /// <summary>
        /// UI图
        /// </summary>
        public byte[] UIImage { get; set; }
        public string ColorHex { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public virtual string Version { get; set; }
        public virtual string Copyright { get; set; }
        public virtual string Company { get; set; }
    }
}
