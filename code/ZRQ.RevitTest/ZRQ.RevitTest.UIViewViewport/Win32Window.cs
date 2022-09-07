using Autodesk.Revit.DB.Macros;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitTest.UIViewViewport
{
    /// <summary>
    /// Win32的一些窗口操作接口
    /// </summary>
    public class Win32Window
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern uint GetWindowLong(IntPtr hwnd, int nIndex);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point pt);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point pt);

        /// <summary>
        /// 窗口状态 最大化最小化操作
        /// 1-正常， FormWindowState.Normal
        /// 2-最小化 FormWindowState.Minimized
        /// 3-最大化 FormWindowState.Maximized
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="cmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="wFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint wFlags);
        /// <summary>
        /// 
        /// </summary>
        public const int HWND_TOP = 0;
        /// <summary>
        /// 
        /// </summary>
        public const int HWND_BOTTOM = 1;
        /// <summary>
        /// 
        /// </summary>
        public const int HWND_TOPMOST = -1;
        /// <summary>
        /// 
        /// </summary>
        public const int HWND_NOTOPMOST = -2;
        /// <summary>
        /// {忽略 cx、cy, 保持大小}
        /// </summary>
        public const uint SWP_NOSIZE = 1;    //
        /// <summary>
        /// {忽略 X、Y, 不改变位置}
        /// </summary>
        public const uint SWP_NOMOVE = 2;    //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idAttach"></param>
        /// <param name="idAttachTo"></param>
        /// <param name="fAttach"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int AttachThreadInput(int idAttach, int idAttachTo, int fAttach);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);//设定焦点

/// <summary>
/// 
/// </summary>
/// <param name="hWnd"></param>
/// <param name="Msg"></param>
/// <param name="wParam"></param>
/// <param name="lParam"></param>
/// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, uint Msg, uint wParam, uint lParam);

/// <summary>
/// 
/// </summary>
/// <param name="lpClassName"></param>
/// <param name="lpWindowName"></param>
/// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        //寻找窗口
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWndParent"></param>
        /// <param name="lpfn"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(int hWndParent, CallBack lpfn, int lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowText(int hWnd, StringBuilder lpString, int nMaxCount);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="bInvert"></param>
        /// <returns></returns>
        [DllImport("user32")]//调用Windows API函数
        public static extern long FlashWindow(IntPtr handle, bool bInvert);


        //具体实现时，首先需要定义以上API函数的回调函数代理：
        /// <summary>
        /// 回调函数代理
        /// </summary>
        public delegate bool CallBack(int hwnd, int lParam);

        const int WM_MOUSEACTIVATE = 0x21;
        private static bool ActiveChildWindowCallBack(int hwnd, int lParam)
        {
            StringBuilder title = new StringBuilder(200);
            int len = GetWindowText(hwnd, title, 200);
            if (len > 0)
            {
                string titleString = title.ToString();
                if (titleString.StartsWith(_sActiveRevitChildWindowTitle))
                {
                    System.Windows.MessageBox.Show(titleString);
                    SendMessage(hwnd, WM_MOUSEACTIVATE, 0, 0);
                    return false;
                }
            }
            return true;
        }

        //
        /// <summary>
        ///  枚举所有子窗口
        /// </summary>
        public static string _sActiveRevitChildWindowTitle = "";
        /// <summary>
        /// 通过名称，激活子窗口
        /// </summary>
        /// <param name="childWindowTitle"></param>
        public static void ActiveRevitChildWindow(string childWindowTitle)
        {
            _sActiveRevitChildWindowTitle = childWindowTitle;
            IntPtr mainWindowHandle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            EnumChildWindows(mainWindowHandle.ToInt32(), new CallBack(ActiveChildWindowCallBack), 0);
        }
        [DllImport("User32.Dll")]
        public static extern void SetWindowText(int h, String s);
        // <summary>
        /// 如果函数执行成功，返回值不为0。
        /// 如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。.NET方法:Marshal.GetLastWin32Error()
        /// <param name="hWnd">要定义热键的窗口的句柄</param>
        /// <param name="id">定义热键ID（不能与其它ID重复）  </param>
        /// <param name="fsModifiers">标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效</param>
        /// <param name="vk">定义热键的内容,WinForm中可以使用Keys枚举转换，
        /// WPF中Key枚举是不正确的,应该使用System.Windows.Forms.Keys枚举，或者自定义正确的枚举或int常量</param>
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, int vk);
        /// <summary>
        /// 取消注册热键
        /// </summary>
        /// <param name="hWnd">要取消热键的窗口的句柄</param>
        /// <param name="id">要取消热键的ID</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        /// <summary>
        /// 向全局原子表添加一个字符串，并返回这个字符串的唯一标识符,成功则返回值为新创建的原子ID,失败返回0
        /// </summary>
        /// <param name="lpString"></param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = true)]
        public static extern short GlobalAddAtom(string lpString);

        [DllImport("kernel32", SetLastError = true)]
        public static extern short GlobalDeleteAtom(short nAtom);

        /// <summary>
        /// 定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）
        /// </summary>
        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }
        /// <summary>
        /// 热键的对应的消息ID
        /// </summary>
        public const int WmHotkey = 0x312;
    }
}
