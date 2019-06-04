using System;
using System.Collections.Generic;
using System.Text;
using Share.Transfe.Args;

namespace Share.Transfe.Delegates
{
    /// <summary>
    /// 进度变化事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="progressChangedArgs"></param>
    public delegate void ProgressChangedHandler(object sender, ProgressChangedArgs progressChangedArgs);
    /// <summary>
    /// 速度变化事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="speedChangedArgs"></param>
    public delegate void SpeedChangedHandler(object sender, SpeedChangedArgs speedChangedArgs);
    /// <summary>
    /// 完成事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="completedArgs"></param>
    public delegate void CompletedHandler(object sender, CompletedArgs completedArgs);
    /// <summary>
    /// 错误事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="errorArgs"></param>
    public delegate void ErrorHandler(object sender, ErrorArgs errorArgs);
}
