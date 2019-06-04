using System;
using System.Collections.Generic;
using System.Text;

namespace Share.Pipe.UpgradePackage
{
    public class PipeUpgradeProgress : Binary<PipeUpgradeProgress>
    {
        public override byte Head => 6;
        /// <summary>
        /// 当次读取大小
        /// </summary>
        public int Read { get; set; }
        /// <summary>
        /// 已经下载大小
        /// </summary>
        public long Loaded { get; set; }
        /// <summary>
        /// 总大小
        /// </summary>
        public long Total { get; set; }
    }
}
