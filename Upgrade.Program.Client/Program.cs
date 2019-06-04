using Share.Pipe;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Upgrade.Program.Client
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            #region 嵌入的DLL加载
            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
            {
                string dllName = e.Name.Contains(",") ? e.Name.Substring(0, e.Name.IndexOf(',')) : e.Name.Replace(".dll", "");
                dllName = dllName.Replace(".", "_");
                if (dllName.EndsWith("_resources")) return null;
                System.Resources.ResourceManager rm = new System.Resources.ResourceManager("Upgrade.Program.Client.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
                byte[] bytes = (byte[])rm.GetObject(dllName);
                return System.Reflection.Assembly.Load(bytes);
            };
            #endregion
            #region 异常处理
            //处理未捕获的异常   
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常   
            Application.ThreadException += (s, e) =>
            {
                if (e.Exception != null)
                {
                    MessageBox.Show(e.Exception.ToString());
                }
            };
            //处理非UI线程异常   
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                if (ex != null)
                {
                    MessageBox.Show(ex.ToString());
                }
            };
            #endregion
            //MessageBox.Show(string.Join("\r\n", args));
            //args = new string[] { "-v", "true", "-u", "http://localhost:9099/api/UpgradeServer", "-pipe", "UpgradeClient", "-um", "54b022e6-5069-4330-8fd6-6c2285903034" };
            Commands.Init(args);
            if (args.Contains("/?") || args.Contains("?"))
            {
                MessageBox.Show(Commands.GetInfo());
            }
            else
            {
                Commands.CommandPipe.Listen();
                //Commands.CommandMainMutex.WaitMainClose();
                if (Commands.CommandMutex.GetMutexFlag())
                {
                    if (Commands.CommandVisible.GetVisible())
                    {
                        try
                        {
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            UpgradeApplication.Run();
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        Commands.CommandPipe.Listen();
                        Application.Run();
                    }
                }
            }
        }
    }
}
