/******************************************************************
* 使本项目源码或本项目生成的DLL前请仔细阅读以下协议内容，如果你同意以下协议才能使用本项目所有的功能，
* * 否则如果你违反了以下协议，有可能陷入法律纠纷和赔偿，作者保留追究法律责任的权利。
* * 1、你可以在开发的软件产品中使用和修改本项目的源码和DLL，但是请保留所有相关的版权信息。
* * 2、不能将本项目源码与作者的其他项目整合作为一个单独的软件售卖给他人使用。
* * 3、不能传播本项目的源码和DLL，包括上传到网上、拷贝给他人等方式。
* * 4、以上协议暂时定制，由于还不完善，作者保留以后修改协议的权利。
* 
*         Copyright (C):       煎饼的归宿
*         CLR版本:             4.0.30319.42000
*         注册组织名:          Microsoft
*         命名空间名称:        Common
*         文件名:              WindowsTools
*         当前系统时间:        2019/2/13 星期三 上午 9:45:29
*         当前登录用户名:      Administrator
*         创建年份:            2019
*         版权所有：           煎饼的归宿QQ：375324644
******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Upgrade.Program.Client
{
    public class WindowsTools
    {
        private struct WindowRect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        public const int HWND_TOP = 0;
        public const int HWND_BOTTOM = 1;
        public const int HWND_TOPMOST = -1;
        public const int HWND_NOTOPMOST = -2;
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint wFlags);
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out WindowRect lpRect);
        [DllImport("user32.dll", EntryPoint = "EnableWindow")]
        private extern static bool EnableWindow(IntPtr hwnd, int bEnabled);
        /// <summary> TopMost,在子线程中设置TopMost=true不管用,用这个api设置就可以
        /// </summary>
        /// <param name="hWnd"></param>
        public static void SetTopomost(IntPtr hWnd)
        {
            WindowRect rect = new WindowRect();
            GetWindowRect(hWnd, out rect);
            SetWindowPos(hWnd, (IntPtr)HWND_TOPMOST, rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top, 0);
        }
        /// <summary> 为窗体设置模态
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="enable"></param>
        public static void EnableWindow(Control control, bool enable)
        {
            EnableWindow(control.Handle, enable ? 1 : 0);
        }
    }
}
