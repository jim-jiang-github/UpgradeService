using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrade.ProgramRelease
{
    public class UpgradeInfo : Xml<UpgradeInfo>
    {
        public FileScan[] FileScans { get; set; }
    }
}
