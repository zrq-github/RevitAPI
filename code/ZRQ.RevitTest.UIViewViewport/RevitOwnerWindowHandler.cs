using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitTest.UIViewViewport
{
    /// <summary>
    /// 获取revit进程窗口句柄
    /// </summary>
    public class RevitOwnerWindowHandler : System.Windows.Forms.IWin32Window
    {
        private IntPtr _hwnd = Autodesk.Windows.ComponentManager.ApplicationWindow;

        /// <summary>
        /// 句柄
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return this._hwnd;
            }
        }
    }
}
