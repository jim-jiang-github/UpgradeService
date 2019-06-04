using System;
using System.Collections.Generic;
using System.Text;

namespace Share.Transfe.Args
{
    public class ErrorArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error">错误内容</param>
        public ErrorArgs(string error)
        {
            Error = error;
        }
        /// <summary>
        /// 错误内容
        /// </summary>
        public string Error { get; set; }
    }
}
