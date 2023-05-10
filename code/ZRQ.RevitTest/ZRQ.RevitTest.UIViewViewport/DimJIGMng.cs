using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitTest.UIViewViewport
{
    /// <summary>
    /// 
    /// </summary>
    internal class DimJigMng : IDisposable
    {
        UIApplication _uiapp = null;

        //刷新计时器
        System.Windows.Forms.Timer _refreshTimer = null;

        //浮动轴符对话框
        DimJigForm _dimJigForm = null;
        UIView _activeUiView = null;

        /// <summary>
        /// 其他数据源
        /// </summary>
        public Object Obj = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiapp"></param>
        public DimJigMng(UIApplication uiapp)
        {
            _uiapp = uiapp;

            //创建透明窗口
            _dimJigForm = new DimJigForm(_uiapp);
            WindowTool.ShowFormModeless(_dimJigForm);

            //焦点
            Win32Window.SetFocus(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);

            //创建定时器
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 100000;
            _refreshTimer.Tick += Timer_Tick_Refresh;

            _activeUiView = UiViewTool.GetActiveUiView(_uiapp);
        }

        /// <summary>
        /// 2018/10/13 李马元 绘制的线需要通过计算得到，需要计算的对象存储在此，
        /// Data4DimBase是数据的基类，不同的功能需要计算的数据对象不同，通过不同的子类，对应不同的功能。
        /// </summary>
        Data4DimBase _data = null;//
        /// <summary>
        /// 要绘制的预览根据传如的参数计算得到
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="data"></param>
        public DimJigMng(UIApplication uiapp, Data4DimBase data)
        {
            _uiapp = uiapp;
            _data = data;
            InitWinDlg();
        }


        /// <summary>
        /// 2019/01/11 李马元 绘制一条直线的预览。传入第一个点
        /// </summary>
        XYZ _firstPt = null;

        /// <summary>
        /// 绘制的预览线是单个直线的构造。传入第一个点
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="firstPt"></param>
        public DimJigMng(UIApplication uiapp, XYZ firstPt)
        {
            _uiapp = uiapp;
            _firstPt = firstPt;
            InitWinDlg();
        }

        void InitWinDlg()
        {
            //创建透明窗口
            _dimJigForm = new DimJigForm(_uiapp);
            WindowTool.ShowFormModeless(_dimJigForm);

            //焦点
            Win32Window.SetFocus(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);

            //创建定时器
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 1000;
            _refreshTimer.Tick += Timer_Tick_Refresh;
            _activeUiView = UiViewTool.GetActiveUiView(_uiapp);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            //销毁窗口
            if (_dimJigForm != null && !_dimJigForm.IsDisposed)
            {
                _dimJigForm.Close();
                _dimJigForm = null;
            }

            //销毁定时器
            if (_refreshTimer != null)
            {
                _refreshTimer.Stop();
                _refreshTimer.Enabled = false;
                _refreshTimer.Dispose();
                _refreshTimer = null;
            }
        }

        /// <summary>
        /// 启动计时器
        /// </summary>
        void StartTimer()
        {
            if (_refreshTimer != null)
            {
                _refreshTimer.Start();
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        void StopTimer()
        {
            if (_refreshTimer != null)
            {
                _refreshTimer.Stop();
            }
        }

        /// <summary>
        /// 绘制JIG委托
        /// </summary>
        public Action<System.Windows.Forms.Form, UIView, System.Drawing.Point, Data4DimBase> DrawJigAction { get; set; }

        /// <summary>
        /// 绘制一条直线的预览 绘制JIG委托
        /// </summary>
        public Action<System.Windows.Forms.Form, UIView, System.Drawing.Point, XYZ> DrawJigActionSinleLine { get; set; }

        /// <summary>
        /// 绘制JIG委托,带自定义数据
        /// </summary>
        public Action<System.Windows.Forms.Form, UIView, System.Drawing.Point, Data4DimBase, Object> DrawJigActionUserData { get; set; }

        /// <summary>
        /// PickPoint 用户ESC，返回空
        /// </summary>
        /// <param name="strTip">revit左下角提示语</param>
        /// <returns></returns>
        public XYZ DoJig(string strTip = "")
        {
            try
            {
                //启动计时器刷新透明窗口
                StartTimer();
                if (!string.IsNullOrEmpty(strTip))
                {
                    return _uiapp.ActiveUIDocument.Selection.PickPoint(strTip);
                }
                else
                {
                    return _uiapp.ActiveUIDocument.Selection.PickPoint();
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                StopTimer();
            }
        }

        /// <summary>
        /// PickPoint 用户ESC，返回空
        /// </summary>
        /// <param name="snapSettings">选择类型枚举，可以取并集。</param>
        /// <param name="strTip">revit左下角提示语</param>
        /// <returns></returns>
        public XYZ DoJig(ObjectSnapTypes snapSettings, string strTip = "")
        {
            try
            {
                //启动计时器刷新透明窗口
                StartTimer();
                try
                {
                    if (!string.IsNullOrEmpty(strTip))
                    {
                        return _uiapp.ActiveUIDocument.Selection.PickPoint(snapSettings, strTip);
                    }
                    else
                    {
                        return _uiapp.ActiveUIDocument.Selection.PickPoint(snapSettings);
                    }
                }
                catch
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                StopTimer();
            }
        }


        /// <summary>
        /// 计时器计时完毕，刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>defult: 默认应该只执行一个委托程序</remarks>
        void Timer_Tick_Refresh(object sender, EventArgs e)
        {
            try
            {
                if (DrawJigAction != null && _activeUiView != null)
                {
                    DrawJigAction(_dimJigForm, _activeUiView, System.Windows.Forms.Cursor.Position, _data);
                }
                else if (DrawJigActionSinleLine != null && _activeUiView != null)
                {
                    DrawJigActionSinleLine(_dimJigForm, _activeUiView, System.Windows.Forms.Cursor.Position, _firstPt);
                }
                else if (DrawJigActionUserData != null && _activeUiView != null)
                {
                    DrawJigActionUserData(_dimJigForm, _activeUiView, System.Windows.Forms.Cursor.Position, _data, Obj);
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}
