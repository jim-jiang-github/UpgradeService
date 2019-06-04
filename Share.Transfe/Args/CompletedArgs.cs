using System;
using System.Collections.Generic;
using System.Text;

namespace Share.Transfe.Args
{
    public class CompletedArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="result">结果内容</param>
        public CompletedArgs(bool success, string result = null)
        {
            Success = success;
            Result = result;
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 结果内容
        /// </summary>
        public string Result { get; set; }
    }
}
