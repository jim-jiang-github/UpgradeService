using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrade.Program.Client
{
    public class CommandVisible : Command
    {
        public override string Name => "是否显示UI(是否可见)";
        public override string Code => "-v";
        public override string Descript => "主要用于设置是否要显示UI界面";
        public CommandVisible()
        {
            this.Value = "true";
        }
        /// <summary>
        /// 是否显示UI
        /// </summary>
        /// <returns></returns>
        public bool GetVisible()
        {
            if (this.Value == "true") { return true; }
            return false;
        }
    }
}
