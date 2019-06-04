using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrade.Program.Client
{
    public static class Commands
    {
        public static CommandMutex CommandMutex { get; } = new CommandMutex();
        public static CommandPipe CommandPipe { get; } = new CommandPipe();
        public static CommandUrl CommandUrl { get; } = new CommandUrl();
        public static CommandVisible CommandVisible { get; } = new CommandVisible();
        public static CommandMainMutex CommandMainMutex { get; } = new CommandMainMutex();
        public static CommandMainRestart CommandMainRestart { get; } = new CommandMainRestart();
        private static Command[] AllCommands { get; } = new Command[] { CommandMutex, CommandPipe, CommandUrl, CommandVisible, CommandMainMutex, CommandMainRestart };
        public static void Init(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Substring(0, 1) == "-" && i + 1 < args.Length && args[i + 1].Substring(0, 1) != "-")
                {
                    string code = args[i++];
                    string value = args[i];
                    Command command = AllCommands.FirstOrDefault(c => c.Code == code);
                    if (command != null)
                    {
                        command.Value = value;
                    }
                }
            }
        }
        public static string GetInfo()
        {
            string info = "命令代码介绍：\r\n" +
                string.Join("\r\n", AllCommands.Select(c => c.ToString()));
            return info;
        }
    }
}
