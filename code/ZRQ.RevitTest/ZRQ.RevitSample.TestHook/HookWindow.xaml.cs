using Autodesk.Revit.UI;
using Gma.System.MouseKeyHook;
using Revit.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZRQ.RevitSample.TestHook
{
    /// <summary>
    /// Interaction logic for LeftUpPlane.xaml
    /// </summary>
    public partial class HookWindow : Window
    {
        private IKeyboardMouseEvents _applHook;

        int _index = 0;

        public UIApplication UiApp { get; }

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

        public HookWindow()
        {
            InitializeComponent();
            this.Closed += HookWindow_Closed;

            _applHook = Hook.AppEvents();
            //applHook.MouseClick += ApplHook_MouseClick;
            //applHook.MouseDown += ApplHook_MouseDown;
            //applHook.MouseDownExt += AppHookMouseDownExt;
            _applHook.MouseUp += ApplHook_MouseUp;
            _applHook.MouseUpExt += ApplHook_MouseUpExt;
            //applHook.KeyPress += AppHookKeyPress;
        }

        private void ApplHook_MouseUpExt(object sender, MouseEventExtArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            e.Handled = true;
            this.tb_Number.Text = $"计数：{_index++}";
        }

        private void ApplHook_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void ApplHook_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            MouseEventExtArgs mouseEventExtArgs = e as MouseEventExtArgs;
            mouseEventExtArgs.Handled = true;
        }

        private void ApplHook_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            MouseEventExtArgs mouseEventExtArgs = e as MouseEventExtArgs;
            mouseEventExtArgs.Handled = true;
        }

        private void HookWindow_Closed(object sender, EventArgs e)
        {
            Unsubscribe();
        }

        public HookWindow(UIApplication application) : this()
        {
            UiApp = application;
        }

        private void AppHookKeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void AppHook_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void AppHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            e.Handled = true;
            this.tb_Number.Text = $"计数：{_index++}";

            //IntPtr hwndRvt = Autodesk.Windows.ComponentManager.ApplicationWindow;
            //PostMessage(hwndRvt, (uint)KEYBOARD_MSG.WM_KEYDOWN, (uint)System.Windows.Forms.Keys.Escape, 0);

            //RevitTask.RunAsync(() =>
            //{
            //    Autodesk.Revit.DB.Transaction transaction = new Autodesk.Revit.DB.Transaction(this.UIApp.ActiveUIDocument.Document, "计数增加");
            //    transaction.Start();

            //    transaction.Commit();
            //});
        }

        public void Unsubscribe()
        {
            _applHook.MouseDownExt -= AppHookMouseDownExt;
            _applHook.KeyPress -= AppHookKeyPress;

            //It is recommened to dispose it
            _applHook.Dispose();
        }
    }
}
