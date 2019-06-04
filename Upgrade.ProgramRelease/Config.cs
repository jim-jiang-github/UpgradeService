using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Upgrade.ProgramRelease
{
    public class Config : Xml<Config>
    {
        /// <summary>
        /// 程序目录
        /// </summary>
        public string ProgramDirectory { get; set; }
        /// <summary> 排除的后缀
        /// </summary>
        public string[] ExcludeExt { get; set; }
        /// <summary> 排除的文件
        /// </summary>
        public string[] ExcludeFile { get; set; }
        /// <summary> 排除的目录
        /// </summary>
        public string[] ExcludeDir { get; set; }
        /// <summary> 包含的文件
        /// </summary>
        public string[] IncludeFile { get; set; }
        /// <summary> 主程序
        /// </summary>
        public string MainExe { get; set; }
        /// <summary> 主程序
        /// </summary>
        public string UpgradeExe { get; set; }
        /// <summary> 只显示不同
        /// </summary>
        public bool DifferenceOnly { get; set; }
        /// <summary> 显示过滤
        /// </summary>
        public bool FilterContain { get; set; }
    }
}
