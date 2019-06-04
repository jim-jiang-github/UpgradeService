using System;
using System.Collections.Generic;
using System.Text;

namespace Share.Pipe.UpgradePackage
{
    public class PipeUpgradeComplete : Binary<PipeUpgradeComplete>
    {
        public override byte Head => 2;
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool Success { get; set; }
    }
}
