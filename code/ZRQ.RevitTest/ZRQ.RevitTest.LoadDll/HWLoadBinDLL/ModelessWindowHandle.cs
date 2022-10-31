using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace HWLoadBinDLL
{
    internal class ModelessWindowHandle : IWin32Window
    {
        private static IntPtr _windowHandle;

        public ModelessWindowHandle()
        {
            Handle = _windowHandle;
        }

        public IntPtr Handle { get; }

        public static void SetHandler(IntPtr handler)
        {
            _windowHandle = handler;
        }

        [DllImport("USER32.DLL")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void BringRevitToFront()
        {
            SetForegroundWindow(_windowHandle);
        }
    }
}
