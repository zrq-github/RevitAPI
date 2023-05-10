using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitTest.UIViewViewport
{
    /// <summary>
    /// 获取WPF进程窗口句柄
    /// </summary>
    public class WPfOwnerWindowHandler : System.Windows.Forms.IWin32Window
    {
        public WPfOwnerWindowHandler(System.Windows.Window window)
        {
            Hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
        }
        private IntPtr Hwnd { get; set; }

        /// <summary>
        /// 句柄
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return this.Hwnd;
            }
        }
    }
}
