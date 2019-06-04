using System;
using System.Collections.Generic;
using System.Text;

namespace Share.Pipe.UpgradePackage
{
    public class PipeUpgradeContent : Binary<PipeUpgradeContent>
    {
        public override byte Head => 3;
        /// <summary>
        /// 升级日志
        /// </summary>
        public string Content { get; set; }
    }
}
