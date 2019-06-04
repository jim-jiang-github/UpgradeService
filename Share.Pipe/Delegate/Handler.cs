using System;
using System.Collections.Generic;
using System.Text;

namespace Share.Pipe.Delegate
{
    /// <summary>
    /// pipe消息事件委托
    /// </summary>
    /// <param name="msg"></param>
    public delegate void PipeMessageHandler(byte[] data);
}
