using System;
using System.Collections.Generic;
using System.Text;

namespace Share.Pipe.UpgradePackage
{
    public class PipeUpgradeCheckVersion : Binary<PipeUpgradeCheckVersion>
    {
        public override byte Head => 0;
        /// <summary>
        /// 请求升级的版本
        /// </summary>
        public string Version { get; set; } 
    }
}
