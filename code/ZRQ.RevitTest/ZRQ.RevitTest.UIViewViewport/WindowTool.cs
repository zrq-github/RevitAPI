using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace ZRQ.RevitTest.UIViewViewport
{
    /// <summary>
    /// 关于WPF窗口的一些操作
    /// </summary>
    public class WindowTool
    {
        /// <summary>
        ///  非模态太显示form.Show(new WPfOwnerWindowHandler(wpfParentWin));
        /// </summary>
        /// <param name="form"></param>
        public static void ShowFormModel_InWpfParent(System.Windows.Forms.Form form, System.Windows.Window wpfParentWin)
        {
            if (form == null)
            {
                return;
            }

            form.Show(new WPfOwnerWindowHandler(wpfParentWin));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        public static void ShowFormModeless(System.Windows.Forms.Form form)
        {
            if (form == null)
            {
                return;
            }

            form.Show(new RevitOwnerWindowHandler());
        }

        /// <summary>
        /// form子窗口，父窗口是wpf窗口
        /// </summary>
        /// <param name="form"></param>
        /// <param name="wpfParentWin"></param>
        /// <returns></returns>
        public static System.Windows.Forms.DialogResult ShowFormModel(System.Windows.Forms.Form form, System.Windows.Window wpfParentWin)
        {
            if (form == null)
            {
                return System.Windows.Forms.DialogResult.Abort;
            }

            return form.ShowDialog(new WPfOwnerWindowHandler(wpfParentWin));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        public static System.Windows.Forms.DialogResult ShowFormModel(System.Windows.Forms.Form form)
        {
            if (form == null)
            {
                return System.Windows.Forms.DialogResult.Abort;
            }

            return form.ShowDialog(new RevitOwnerWindowHandler());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wpfWin"></param>
        public static void ShowWindowModeless(Window wpfWin)
        {
            if (wpfWin == null)
            {
                return;
            }

            System.Windows.Interop.WindowInteropHelper x = new System.Windows.Interop.WindowInteropHelper(wpfWin);
            x.Owner = Autodesk.Windows.ComponentManager.ApplicationWindow;

            wpfWin.WindowState = WindowState.Normal;
            wpfWin.Activate();
            wpfWin.Show();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wpfWin"></param>
        /// <returns></returns>
        public static bool? ShowWindowModel(Window wpfWin)
        {
            if (wpfWin == null)
            {
                return null;
            }
            System.Windows.Interop.WindowInteropHelper x = new System.Windows.Interop.WindowInteropHelper(wpfWin);

            x.Owner = Autodesk.Windows.ComponentManager.ApplicationWindow;

            //2020 10 29 注释原因  : 在自定义的dockpanel显示后 如果一段时间未使用revit情况下 点击按钮或者任何能触发弹框的调用时 会导致窗口一闪及时并卡死
            //x.Owner = Process.GetCurrentProcess().MainWindowHandle;

            //2018/06/22 李马元 昨天晚上加的两行设置导致wpfWin置最前，
            //如果wpfWin的子窗口不是用这个接口ShowDialog，会被父窗口遮挡，
            //注释掉这两行
            //wpfWin.Topmost = true;
            //wpfWin.Focus();
            wpfWin.WindowState = WindowState.Normal;
            wpfWin.Activate();
            bool? bRet = wpfWin.ShowDialog();

            return bRet;
        }
        /// <summary>
        ///2020/01/10 李马元  设置wpf窗口父窗口为revit主窗口，模态框显示
        /// </summary>
        /// <param name="wpfWin"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static bool? ShowDialog(Window wpfWin)
        {
            try
            {
                //2020 10 29 注释原因  : 在自定义的dockpanel显示后 如果一段时间未使用revit情况下 点击按钮或者任何能触发弹框的调用时 会导致窗口一闪及时并卡死
                //System.Windows.Interop.WindowInteropHelper x = new System.Windows.Interop.WindowInteropHelper(wpfWin);
                //x.Owner = Process.GetCurrentProcess().MainWindowHandle;

                if (wpfWin == null)
                {
                    return null;
                }
                System.Windows.Interop.WindowInteropHelper x = new System.Windows.Interop.WindowInteropHelper(wpfWin);

                x.Owner = Autodesk.Windows.ComponentManager.ApplicationWindow;
                wpfWin.WindowState = WindowState.Normal;
                wpfWin.Activate();
                bool? bRet = wpfWin.ShowDialog();
                return bRet;
            }
            catch (Exception ex) 
            {
                return null;
            }
        }

        /// <summary>
        ///2020/01/10 李马元  设置wpf窗口父窗口为winform窗口，模态框显示
        /// </summary>
        /// <param name="wpfWin"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static bool? ShowDialog(Window wpfWin, System.Windows.Forms.Form parent)
        {
            try
            {
                System.Windows.Interop.WindowInteropHelper x = new System.Windows.Interop.WindowInteropHelper(wpfWin);
                x.Owner = parent.Handle;
                bool? bRet = wpfWin.ShowDialog();
                return bRet;
            }
            catch (Exception ex) 
            {
                return null;
            }
        }

        /// <summary>
        ///2020/01/10 李马元  设置wpf窗口父窗口为UserControl 控件，模态框显示
        /// wpfWin.Owner = Window.GetWindow(parent);
        /// </summary>
        /// <param name="wpfWin"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static bool? ShowDialog(Window wpfWin, UserControl parent)
        {
            try
            {
                wpfWin.Owner = Window.GetWindow(parent);
                bool? bRet = wpfWin.ShowDialog();
                return bRet;
            }
            catch (Exception ex) 
            {
                return null;
            }
        }

        /// <summary>
        ///2020/01/10 李马元  设置wpf窗口父窗口为UserControl 控件，模态框显示
        ///一键XX的子窗口是UserControl
        /// </summary>
        /// <param name="wpfWin"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static bool? ShowDialog(Window wpfWin, System.Windows.Forms.UserControl parent)
        {
            try
            {
                System.Windows.Interop.WindowInteropHelper x = new System.Windows.Interop.WindowInteropHelper(wpfWin);
                x.Owner = parent.Handle;
                bool? bRet = wpfWin.ShowDialog();
                return bRet;
            }
            catch (Exception ex) 
            {
                return null;
            }
        }

        /// <summary>
        /// 2020/12/14 李马元  设置winform窗口父窗口为winform窗口，模态框显示
        /// </summary>
        /// <param name="winformDlg"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static System.Windows.Forms.DialogResult ShowDialog(System.Windows.Forms.Form winformDlg, System.Windows.Forms.Form parent)
        {
            try
            {
                return winformDlg.ShowDialog(parent);
            }
            catch (Exception)
            {
                return System.Windows.Forms.DialogResult.Abort;
            }
        }

        /// <summary>
        ///WPF查找子控件和父控件方法
        ///一、查找某种类型的子控件，并返回一个List集合
        ///调用：
        ///List<Button> listButtons = GetChildObjects<Button>(parentPanel, typeof(Button));
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(DependencyObject obj, Type typename) where T : FrameworkElement
        {
            if (obj == null)
            {
                return null;
            }

            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).GetType() == typename))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, typename));
            }
            return childList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(DependencyObject obj) where T : FrameworkElement
        {
            if (obj == null)
            {
                return null;
            }

            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child));
            }
            return childList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listView"></param>
        /// <returns></returns>
        public static List<ListViewItem> GetSelectListViewItem(ListView listView)
        {
            if (listView == null)
            {
                return null;
            }

            List<ListViewItem> allItems = GetChildObjects<ListViewItem>(listView);
            List<ListViewItem> allSelects = new List<ListViewItem>();
            foreach (ListViewItem item in allItems)
            {
                if (item.IsSelected)
                {
                    allSelects.Add(item);
                }
            }

            return allSelects;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeView"></param>
        /// <returns></returns>
        public static List<TreeViewItem> GetSelectTreeViewItem(TreeView treeView)
        {
            if (treeView == null)
            {
                return null;
            }

            List<TreeViewItem> allItems = GetChildObjects<TreeViewItem>(treeView);
            List<TreeViewItem> allSelects = new List<TreeViewItem>();
            foreach (TreeViewItem item in allItems)
            {
                if (item.IsSelected)
                {
                    allSelects.Add(item);
                }
            }

            return allSelects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabControl"></param>
        /// <returns></returns>
        public static TabItem GetSelectTabItem(TabControl tabControl)
        {
            if (tabControl == null)
            {
                return null;
            }

            List<TabItem> allItems = GetChildObjects<TabItem>(tabControl);
            foreach (TabItem item in allItems)
            {
                if (item.IsSelected)
                {
                    return item;
                }
            }

            return null;
        }


        /// <summary>
        /// 二、通过名称查找子控件，并返回一个List集合
        /// 调用：
        /// List Button listButtons = GetChildObjects Button(parentPanel, "button1");
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// </summary>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            if (obj == null)
            {
                return null;
            }

            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).GetType().ToString() == name | string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, name));
            }
            return childList;
        }



        /// <summary>
        ///三、通过名称查找某子控件：
        ///调用：
        ///StackPanel sp = GetChildObject(this.LayoutRoot, "spDemoPanel");
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            if (obj == null)
            {
                return null;
            }

            DependencyObject child = null;
            T grandChild = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

        public static T GetFirstChildObject<T>(DependencyObject obj) where T : FrameworkElement
        {
            if (obj == null)
            {
                return null;
            }

            DependencyObject child = null;
            T grandChild = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetFirstChildObject<T>(child);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }


        //四、通过名称查找父控件
        //调用：
        //Grid layoutGrid = VTHelper.GetParentObject(this.spDemoPanel, "LayoutRoot");
        public static T GetParentObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            if (obj == null)
            {
                return null;
            }

            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {
                if (parent is T && (((T)parent).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)parent;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        public static T GetFirstParentObject<T>(DependencyObject obj) where T : FrameworkElement
        {
            if (obj == null)
            {
                return null;
            }

            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }
        /// <summary>
        /// 设置WPF窗口的最小最大按钮现实与隐藏
        /// </summary>
        /// <param name="window"></param>
        /// <param name="isEnableMax"></param>
        public static void SetMaxmizeboxEnable(System.Windows.Window window, bool isEnableMax, bool isEnableMin)
        {
            int gwlStyle = -16;
            uint wsMaximizebox = 0x00010000;
            uint wsMinimizebox = 0x00020000;
            uint swpNosize = 0x0001;
            uint swpNomove = 0x0002;
            uint swpFramechanged = 0x0020;
            IntPtr handle = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            uint nStyle = Win32Window.GetWindowLong(handle, gwlStyle);
            if (!isEnableMax)
            {
                nStyle &= ~(wsMaximizebox);
            }
            else
            {
                nStyle |= wsMaximizebox;
            }
            if (!isEnableMin)
            {
                nStyle &= ~(wsMinimizebox);
            }
            else
            {
                nStyle |= wsMinimizebox;
            }
           Win32Window.SetWindowLong(handle, gwlStyle, (IntPtr)((long)((ulong)nStyle)));
           Win32Window.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, swpNosize | swpNomove | swpFramechanged);
        }
    }
}
