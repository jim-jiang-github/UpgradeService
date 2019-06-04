using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgrade.Program.Client
{
    public class CommandMainMutex : Command
    {
        public override string Name => "主程序互斥锁";
        public override string Code => "-mm";
        public override string Descript => "主要用于判读主程序是否关闭，如果主程序关闭的话升级程序也要关闭，为空则说明不需跟随主程序的关闭而关闭";
        /// <summary>
        /// 执行等待主程序的关闭
        /// </summary>
        /// <returns></returns>
        public bool WaitMainClose()
        {
            if (this.Value != null)
            {
                if (Mutex.TryOpenExisting(this.Value, out Mutex m))
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            m.WaitOne(); //等待主线程退出
                            Application.Exit();
                        }
                        catch // 主线程退出之后会报错“由于出现被放弃的 mutex,等待过程结束”
                        {
                            Application.Exit();
                        }
                        finally
                        {
                            m.Dispose();
                        }
                    });
                    return true;
                }
                else
                {
                    //在升级程序启动之前 主程序就关闭了 升级程序也关闭
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
