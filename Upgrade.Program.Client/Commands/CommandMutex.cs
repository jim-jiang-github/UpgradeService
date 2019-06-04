using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrade.Program.Client
{
    public class CommandMutex : Command
    {
        private static System.Threading.Mutex mutex = null; //这里要定义成静态的，不然会有bug
        public override string Name => "升级程序互斥锁";
        public override string Code => "-um";
        public override string Descript => "主要用于让升级程序进程只能存在一个，也可以用来判断是否打开升级程序";
        /// <summary>
        /// 是否互斥 true 是不存在互斥 false是互斥
        /// </summary>
        /// <returns></returns>
        public bool GetMutexFlag()
        {
            if (this.Value == null) { return false; }
            mutex?.Dispose();
            mutex = new System.Threading.Mutex(true, this.Value, out bool ret);
            return ret;
        }
    }
}
