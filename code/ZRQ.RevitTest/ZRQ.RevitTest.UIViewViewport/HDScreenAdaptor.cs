using Autodesk.Revit.DB.Macros;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZRQ.RevitTest.UIViewViewport
{
    /// <summary>
    /// 适配高清屏幕
    /// </summary>
    /// <remarks>
    /// 这个模块, 不应该在这里, 要转移到HWCommonUI中去(lwc)
    /// </remarks>
    public static class HDScreenAdaptor
    {
        #region Win32 API
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(
IntPtr hdc, // handle to DC
            int nIndex // index of capability
        );
        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
        #endregion

        #region DeviceCaps常量
        const int HORZRES = 8;
        const int VERTRES = 10;
        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;
        #endregion

        #region 属性
        /// <summary>
        /// 当前系统DPI_X 大小 一般为96
        /// </summary>
        public static int DpiX
        {
            get
            {
                var hdc = GetDC(IntPtr.Zero);
                var dpiX = GetDeviceCaps(hdc, LOGPIXELSX);
                ReleaseDC(IntPtr.Zero, hdc);
                return dpiX;
            }
        }
        public static int Magnify
        {
            get
            {
                var hdc = GetDC(IntPtr.Zero);
                var dpiX = GetDeviceCaps(hdc, LOGPIXELSX);
                ReleaseDC(IntPtr.Zero, hdc);
                return Convert.ToInt32(dpiX / 96f * 100);
            }
        }
        /// <summary>
        /// 当前系统DPI_Y 大小 一般为96
        /// </summary>
        public static int DpiY
        {
            get
            {
                var hdc = GetDC(IntPtr.Zero);
                var dpiX = GetDeviceCaps(hdc, LOGPIXELSY);
                ReleaseDC(IntPtr.Zero, hdc);
                return dpiX;
            }
        }

        /// <summary>
        /// 获取真实设置的桌面分辨率大小
        /// </summary>
        public static Size Desktop
        {
            get
            {
                var hdc = GetDC(IntPtr.Zero);
                var size = new Size
                {
                    Width = GetDeviceCaps(hdc, DESKTOPHORZRES),
                    Height = GetDeviceCaps(hdc, DESKTOPVERTRES)
                };
                ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }

        /// <summary>
        /// 获取屏幕分辨率当前物理大小
        /// </summary>
        public static Size WorkingArea
        {
            get
            {
                var hdc = GetDC(IntPtr.Zero);
                var size = new Size
                {
                    Width = GetDeviceCaps(hdc, HORZRES),
                    Height = GetDeviceCaps(hdc, VERTRES)
                };
                ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }

        /// <summary>
        /// 获得横轴缩放比
        /// 此参数为[控制面板-设置-显示-更改文本、应用等项目的大小]所显示的百分比
        /// </summary>
        public static float ScaleX => DpiX / 96f;

        /// <summary>
        /// 获得纵轴缩放比
        /// 此参数为[控制面板-设置-显示-更改文本、应用等项目的大小]所显示的百分比
        /// </summary>
        public static float ScaleY => DpiY / 96f;
        #endregion
        /// <summary>
        /// scale比
        /// </summary>
        /// <returns></returns>
        public static float GetPositionScale()
        {
            switch (Magnify)
            {
                case 125:
                    return .8f;
                case 150:
                    return .65f;
                case 175:
                    return .575f;
                default:
                    return 1f;
            }
        }
        private static List<Form> _buffer { get; set; } = new List<Form>();
        /// <summary>
        /// 适配winform的高清屏
        /// </summary>
        public static void AdaptHDScreen(Control ctrl, string[] ExcludeControl = null)
        {
            //用户电脑情况太多了，发布后遇到各种情况，暂时先去掉
#if false
            if (null == ctrl)
            {
                return;
            }

            if (ctrl is Form && _buffer.Contains(ctrl))
            {
                return;
            }

            if (ctrl is Form)
            {
                Form fm = ctrl as Form;
                _buffer.Add(fm);
                fm.FormClosed += Fm_FormClosed;

                //主窗口            
                ctrl.ClientSize = ctrl.ClientSize.AdaptHDScreen();
                ctrl.MinimumSize = ctrl.MinimumSize.AdaptHDScreen();
                fm.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            }
            else
            {
                if (null != ExcludeControl && ExcludeControl.Contains(ctrl.Name))
                {

                }
                else
                {
                    ctrl.AutoSize = false;
                    ctrl.Size = ctrl.Size.AdaptHDScreen();
                    ctrl.Location = ctrl.Location.AdaptHDScreen();
                }
            }
            //遍历子控件
            for (int i = 0; i < ctrl.Controls.Count; i++)
            {
                Control control = ctrl.Controls[i];
                AdaptHDScreen(control, ExcludeControl);
            }
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Fm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form fm = sender as Form;
            if (_buffer.Contains(fm))
            {
                _buffer.Remove(fm);
            }
        }

        /// <summary>
        /// Size的高清适配
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static System.Drawing.Size AdaptHDScreen(this System.Drawing.Size size)
        {
            System.Drawing.Size output = new System.Drawing.Size()
            {
                Width = (int)(size.Width * GetMagnify() / 100),
                Height = (int)(size.Height * GetMagnify() / 100),
            };
            return output;
        }
        /// <summary>
        /// Point的高清适配
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static System.Drawing.Point AdaptHDScreen(this System.Drawing.Point point)
        {
            System.Drawing.Point output = new System.Drawing.Point()
            {
                X = (int)(point.X * GetMagnify() / 100),
                Y = (int)(point.Y * GetMagnify() / 100),
            };
            return output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static double GetMagnify()
        {
            return Magnify;
        }
    }
}
