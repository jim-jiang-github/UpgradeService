using System;
using System.Collections.Generic;
using System.Text;

namespace Share.Pipe.UpgradePackage
{
    public class PipeUpgradeError : Binary<PipeUpgradeError>
    {
        public override byte Head => 4;
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Error { get; set; }
    }
}
