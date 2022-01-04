/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/3 17:34:15
 * 文件描述:  
 * 
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RQ.Test.RevtDotNet
{
    internal class ModelessWindowHandle : IWin32Window
    {
        private static IntPtr _revitMainWindowHandle;

        public ModelessWindowHandle()
        {
            Handle = _revitMainWindowHandle;
        }

        public IntPtr Handle { get; }

        public static void SetHandler(IntPtr handler)
        {
            _revitMainWindowHandle = handler;
        }

        [DllImport("USER32.DLL")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void BringRevitToFront()
        {
            SetForegroundWindow(_revitMainWindowHandle);
        }
    }
}