using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZRQ.RevitTest.UIViewViewport
{
    public partial class DimJIGForm : Form
    {
        UIApplication _uiapp = null;

        public DimJIGForm(UIApplication uiapp)
        {
            InitializeComponent();

            this.TransparencyKey = this.BackColor;
            this.ShowInTaskbar = false;

            //窗口大小,位置
            _uiapp = uiapp;

            uint num = Win32Window.GetWindowLong(base.Handle, -20);//GWL_EXSTYLE
            num |= 32u;//WS_EX_TRANSPARENT
            num |= 524288u;//WS_EX_LAYERED
            Win32Window.SetWindowLong(base.Handle, -20, (IntPtr)((long)((ulong)num)));

            this.Load += FormLoaded;
        }

        private void FormLoaded(object sender, EventArgs e)
        {
            UIView uiview = UIViewTool.GetActiveUIView(_uiapp);
            if (uiview != null)
            {
                var viewRect = uiview.GetWindowRectangle();
                this.Location = new Point(viewRect.Left, viewRect.Top);
                this.Size = new Size(viewRect.Right - viewRect.Left, viewRect.Bottom - viewRect.Top);
            }
             HDScreenAdaptor.AdaptHDScreen(this);
        }
    }
}
