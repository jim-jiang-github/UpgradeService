using System;
using System.Collections.Generic;
using System.Text;

namespace Share.Transfe.Args
{
    public class ProgressChangedArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="read">当次读取</param>
        /// <param name="loaded">已读取</param>
        /// <param name="total">总长度</param>
        public ProgressChangedArgs(int read, long loaded, long total)
        {
            Read = read;
            Loaded = loaded;
            Total = total;
        }
        /// <summary>
        /// 当次读取
        /// </summary>
        public int Read { get; set; }
        /// <summary>
        /// 已读取
        /// </summary>
        public long Loaded { get; set; }
        /// <summary>
        /// 总长度
        /// </summary>
        public long Total { get; set; }
        public override string ToString()
        {
            return "Read:" + Read + "|" + "Loaded:" + Loaded + "|" + "Total:"+ Total;
        }
    }
}
