using System;
using System.Collections.Generic;
using System.Text;

namespace Share.Pipe.UpgradePackage
{
    public class PipeUpgradeMessage : Binary<PipeUpgradeMessage>
    {
        public override byte Head => 5;
        /// <summary>
        /// 升级消息
        /// </summary>
        public string Message { get; set; }
    }
}
