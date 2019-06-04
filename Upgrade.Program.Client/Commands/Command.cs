using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrade.Program.Client
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract string Code { get; }
        public abstract string Descript { get; }
        public virtual string Value { get; set; }
        public override string ToString()
        {
            return Code + "：" + Name + "(" + Descript + ")";
        }
    }
}
