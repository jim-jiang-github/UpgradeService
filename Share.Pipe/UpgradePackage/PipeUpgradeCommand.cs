using System;
using System.Collections.Generic;
using System.Text;

namespace Share.Pipe.UpgradePackage
{
    public class PipeUpgradeCommand : Binary<PipeUpgradeCommand>
    {
        public override byte Head => 1;
        public enum Commands
        {
            /// <summary>
            /// 关闭程序
            /// </summary>
            Colse = 1
        }
        /// <summary>
        /// 命令
        /// </summary>
        public Commands Command { get; set; }
    }
}
