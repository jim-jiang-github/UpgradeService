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
    public class CommandMainRestart : Command
    {
        public override string Name => "主程序重启";
        public override string Code => "-mr";
        public override string Descript => "主要用于重启主程序,输入参数是主程序名,需要配合主程序互斥锁使用";
        /// <summary>
        /// 执行等待主程序的打开
        /// </summary>
        /// <returns></returns>
        public async Task<bool> MainStart()
        {
            if (this.Value == null) { return false; }
            Process.Start(this.Value);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(5000);
            return await Task<bool>.Run(async () =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    if (Mutex.TryOpenExisting(Commands.CommandMainMutex.Value, out Mutex m))
                    {
                        m?.Dispose();
                        break;
                    }
                    await Task.Delay(100);
                }
                return !cancellationTokenSource.IsCancellationRequested;
            }, cancellationTokenSource.Token);
        }
    }
}
