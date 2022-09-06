using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using HWTransCommon.GeometricTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitTest.UIViewViewport
{
    public class UIViewTool
    {

        /// <summary>
        /// 当前uiview必须要有焦点，不然revit的选择，会抛出异常
        ///从工具条进去后，点击修改菜单中的命令，此时uiview是没有焦点的
        ///使当前视图的uiview重新获得焦点
        /// </summary>
        /// <param name="application"></param>
        static public void ChangeViewFocus(UIApplication application)
        {
            try
            {
                //当前激活视图
                Autodesk.Revit.DB.View view = application.ActiveUIDocument.ActiveView;
                if (!IsSupportViewType(view))
                {//2021/03/17 李马元 出现死循环bug，某些视图不支持选择操作。一键xx的选择循环就一直进异常，异常中进入这里切换视图焦点，还是进入不支持选择操作的视图。
                    return;
                }

                IList<UIView> openUIViews = application.ActiveUIDocument.GetOpenUIViews();
                foreach (UIView uiViewOther in openUIViews)
                {
                    if ((view == null) || (uiViewOther.ViewId.IntegerValue != view.Id.IntegerValue))
                    {
                        var otherView = application.ActiveUIDocument.Document.GetElement(uiViewOther.ViewId) as Autodesk.Revit.DB.View;
                        if (!IsSupportViewType(otherView))
                        {
                            continue;
                        }
                        application.ActiveUIDocument.ActiveView = otherView;
                        application.ActiveUIDocument.ActiveView = view;
                        return;
                    }
                }

                FilteredElementCollector collector = new FilteredElementCollector(application.ActiveUIDocument.Document);
                FilteredElementIterator elementIterator = collector.OfClass(typeof(Autodesk.Revit.DB.View)).GetElementIterator();
                elementIterator.Reset();
                while (elementIterator.MoveNext())
                {
                    Autodesk.Revit.DB.View otherView = elementIterator.Current as Autodesk.Revit.DB.View;
                    if (otherView != null &&
                        !otherView.IsTemplate &&
                        otherView.Id.IntegerValue != view.Id.IntegerValue &&
                        IsSupportViewType(otherView))
                    {
                        application.ActiveUIDocument.ActiveView = otherView;
                        application.ActiveUIDocument.ActiveView = view;
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                string message = exception.Message;
            }
        }

        /// <summary>
        /// 2021/03/017 李马元 抽象为一个方法
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static bool IsSupportViewType(View view)
        {
            if (view != null &&
                       !view.IsTemplate &&
                       (view.ViewType == ViewType.ThreeD ||
                       view.ViewType == ViewType.FloorPlan ||
                       view.ViewType == ViewType.EngineeringPlan ||
                       view.ViewType == ViewType.CeilingPlan ||
                       view.ViewType == ViewType.Elevation ||
                       view.ViewType == ViewType.Section))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取当前激活的UI视图
        /// </summary>
        /// <param name="uiapp"></param>
        /// <returns></returns>
        static public UIView GetActiveUIView(UIApplication uiapp)
        {
            if (uiapp == null ||
                uiapp.ActiveUIDocument == null)
            {
                return null;
            }

            Autodesk.Revit.DB.View activeView = uiapp.ActiveUIDocument.ActiveView;
            if (activeView != null)
            {
                IList<UIView> allUiViews = uiapp.ActiveUIDocument.GetOpenUIViews();
                foreach (var uiview in allUiViews)
                {
                    if (uiview.ViewId == activeView.Id)
                    {
                        return uiview;
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="iLeft"></param>
        /// <param name="iTop"></param>
        static public void GetCurrentViewLeftUpPos(UIApplication uiapp, ref int iLeft, ref int iTop)
        {
            UIView uiview = GetActiveUIView(uiapp);
            if (uiview != null)
            {
#if REVIT2014 || REVIT2015 || REVIT2016//2018/05/10 李马元 命名空间问题
                var viewRect = uiview.GetWindowRectangle();
                iLeft = viewRect.Left;
                iTop = viewRect.Top;
#else
                var viewRect = uiview.GetWindowRectangle();
                iLeft = viewRect.Left;
                iTop = viewRect.Top;
#endif
            }
            else
            {
#if REVIT2014 || REVIT2015 || REVIT2016
                var rect = uiapp.MainWindowExtents;
                iLeft = rect.Left + 200;
                iTop = rect.Top + 100;
#else
                var rect = uiapp.MainWindowExtents;
                iLeft = rect.Left + 200;
                iTop = rect.Top + 100;
#endif
            }
        }

        internal static System.Drawing.Point ViewPlan2Screen_ActiveView(XYZ realPtOnView, UIView uiview, Viewport viewPort, View view, ViewSheet viewSheet)
        {
            // view 是视口内的视图
            // viewPort 是视口
            // activeGraphicalView 应该是等于 view 的

            var corners = uiview.GetZoomCorners();
            XYZ wLeftBottom = corners[0];
            XYZ wRightTop = corners[1];

            // zrq: 获取视口内的视图方向
            XYZ upDirection = view.UpDirection;
            XYZ rightDirection = view.RightDirection;
            XYZ viewDirection = view.ViewDirection;

            // zrq: 定义矩阵, 来源于视口内的视图
            Transform transform = Transform.Identity;
            transform.BasisX = rightDirection;
            transform.BasisY = upDirection;
            transform.BasisZ = viewDirection;
            transform.Origin = view.Origin;

            // 视口内视图
            BoundingBoxUV Voln = view.Outline;
            var Voln_cen = (Voln.Min + Voln.Max) / 2;
            int Scale = view.Scale;

            // 视口相关
            Outline VPoln = viewPort.GetBoxOutline();
            XYZ VPcen = (VPoln.MaximumPoint + VPoln.MinimumPoint) / 2;
            VPcen = new XYZ(VPcen.X, VPcen.Y, 0);

            // Correction offset from VCen to centre of Viewport in sheet coords 
            XYZ Offset = VPcen - new XYZ(Voln_cen.U, Voln_cen.V, 0);

            // zrq: 开始做偏移处理
            realPtOnView = transform.Inverse.OfPoint(realPtOnView);
            realPtOnView = realPtOnView.Multiply((double)1 / Scale);
            realPtOnView = realPtOnView + Offset;

            wLeftBottom = transform.Inverse.OfPoint(wLeftBottom).Multiply((double)1 / Scale);
            wRightTop = transform.Inverse.OfPoint(wRightTop).Multiply((double)1 / Scale);

            // 计算预览线的一些东西
            var plane = PlaneExtension.CreatePlane(viewSheet.ViewDirection.Normalize(), PlaneExtension.GetProjectedOriginPoint(viewSheet.ViewDirection.Normalize(), wLeftBottom));
            var point = PlaneExtension.Project(plane, realPtOnView);
            Line lineup = Line.CreateUnbound(wLeftBottom, viewSheet.UpDirection);
            Line lineright = Line.CreateUnbound(wLeftBottom, viewSheet.RightDirection);

            double wWidth = lineright.Project(wRightTop).XYZPoint.DistanceTo(lineright.Project(wLeftBottom).XYZPoint);
            double wHeight = lineup.Project(wRightTop).XYZPoint.DistanceTo(lineup.Project(wLeftBottom).XYZPoint);

            int symbol = 1;
            var proPoint = lineright.Project(point).XYZPoint;
            var proPoint2 = lineup.Project(point).XYZPoint;
            if (!(point - proPoint2).Normalize().IsAlmostEqualTo(viewSheet.RightDirection, 0.01))
            {
                symbol = -1;
            }
            double widthScale = symbol * (proPoint.DistanceTo(lineright.Project(wLeftBottom).XYZPoint) / wWidth);

            symbol = 1;

            if (!(point - proPoint).Normalize().IsAlmostEqualTo(viewSheet.UpDirection, 0.01))
            {
                symbol = -1;
            }
            double heightScale = symbol * (proPoint2.DistanceTo(lineup.Project(wLeftBottom).XYZPoint) / wHeight);

            //屏幕坐标
            var rect = uiview.GetWindowRectangle();
            //屏幕比例
            double sWidht = rect.Right - rect.Left;
            double sHeight = rect.Bottom - rect.Top;

            double widthDis = sWidht * widthScale;
            double heightDis = sHeight * heightScale;

            return new System.Drawing.Point((int)(rect.Left + widthDis), (int)(rect.Bottom - heightDis));
        }

        /// <summary>
        /// 屏幕坐标到revit平面坐标转换
        /// 支持平剖面。
        /// </summary>
        /// <param name="screenPoint"></param>
        /// <param name="uiview"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        static public XYZ Screen2ViewPlan(System.Drawing.Point screenPoint, UIView uiview, View view)
        {
            //屏幕坐标
            var rect = uiview.GetWindowRectangle();
            //屏幕比例
            double sWidht = rect.Right - rect.Left;
            double sHeight = rect.Bottom - rect.Top;

            double widhtScale = (screenPoint.X - rect.Left) / sWidht;
            double heightScale = (rect.Bottom - screenPoint.Y) / sHeight;

            var corners = uiview.GetZoomCorners();
            XYZ wLeftBottom = corners[0];
            XYZ wRightTop = corners[1];

            var Orgin = view.Origin;

            var right = view.RightDirection;
            Line lineup = Line.CreateUnbound(Orgin, view.UpDirection);
            Line lineright = Line.CreateUnbound(Orgin, right);

            double wWidth = lineright.Project(wRightTop).XYZPoint.DistanceTo(lineright.Project(wLeftBottom).XYZPoint);
            double wHeight = lineup.Project(wRightTop).XYZPoint.DistanceTo(lineup.Project(wLeftBottom).XYZPoint);

            double widthDis = wWidth * widhtScale;
            double heightDis = wHeight * heightScale;
            var point = wLeftBottom + widthDis * right + heightDis * view.UpDirection;

            return point;
        }

        /// <summary>
        /// revit平面坐标到屏幕坐标转化,
        /// 支持平剖面。
        /// </summary>
        /// <param name="realPtOnView"></param>
        /// <param name="uiview"></param>
        /// <param name="activeGraphicalView"></param>
        /// <returns></returns>
        static public System.Drawing.Point ViewPlan2Screen_noActiveView(XYZ realPtOnView, UIView uiview, View activeGraphicalView, Viewport viewPort, View view, ViewSheet viewSheet)
        {
            // view 是视口内的视图
            // viewPort 是视口
            // activeGraphicalView 应该是等于 view 的

            var corners = uiview.GetZoomCorners();
            XYZ wLeftBottom = corners[0];
            XYZ wRightTop = corners[1];

            // zrq: 获取视口内的视图方向
            XYZ upDirection = view.UpDirection;
            XYZ rightDirection = view.RightDirection;
            XYZ viewDirection = view.ViewDirection;

            // zrq: 定义矩阵, 来源于视口内的视图
            Transform transform = Transform.Identity;
            transform.BasisX = rightDirection;
            transform.BasisY = upDirection;
            transform.BasisZ = viewDirection;
            transform.Origin = view.Origin;

            // 视口内视图
            BoundingBoxUV Voln = view.Outline;
            var Voln_cen = (Voln.Min + Voln.Max) / 2;
            int Scale = view.Scale;

            // 视口相关
            Outline VPoln = viewPort.GetBoxOutline();
            XYZ VPcen = (VPoln.MaximumPoint + VPoln.MinimumPoint) / 2;
            VPcen = new XYZ(VPcen.X, VPcen.Y, 0);

            // Correction offset from VCen to centre of Viewport in sheet coords 
            XYZ Offset = VPcen - new XYZ(Voln_cen.U, Voln_cen.V, 0);

            // zrq: 开始做偏移处理
            realPtOnView = transform.Inverse.OfPoint(realPtOnView);
            realPtOnView = realPtOnView.Multiply((double)1 / Scale);
            realPtOnView = realPtOnView + Offset;


            // 计算预览线的一些东西
            var plane = PlaneExtension.CreatePlane(viewSheet.ViewDirection.Normalize(), PlaneExtension.GetProjectedOriginPoint(viewSheet.ViewDirection.Normalize(), wLeftBottom));
            var point = PlaneExtension.Project(plane, realPtOnView);
            Line lineup = Line.CreateUnbound(wLeftBottom, viewSheet.UpDirection);
            Line lineright = Line.CreateUnbound(wLeftBottom, viewSheet.RightDirection);

            double wWidth = lineright.Project(wRightTop).XYZPoint.DistanceTo(lineright.Project(wLeftBottom).XYZPoint);
            double wHeight = lineup.Project(wRightTop).XYZPoint.DistanceTo(lineup.Project(wLeftBottom).XYZPoint);

            int symbol = 1;
            var proPoint = lineright.Project(point).XYZPoint;
            var proPoint2 = lineup.Project(point).XYZPoint;
            if (!(point - proPoint2).Normalize().IsAlmostEqualTo(viewSheet.RightDirection, 0.01))
            {
                symbol = -1;
            }
            double widthScale = symbol * (proPoint.DistanceTo(lineright.Project(wLeftBottom).XYZPoint) / wWidth);

            symbol = 1;

            if (!(point - proPoint).Normalize().IsAlmostEqualTo(viewSheet.UpDirection, 0.01))
            {
                symbol = -1;
            }
            double heightScale = symbol * (proPoint2.DistanceTo(lineup.Project(wLeftBottom).XYZPoint) / wHeight);

            //屏幕坐标
            var rect = uiview.GetWindowRectangle();
            //屏幕比例
            double sWidht = rect.Right - rect.Left;
            double sHeight = rect.Bottom - rect.Top;

            double widthDis = sWidht * widthScale;
            double heightDis = sHeight * heightScale;

            return new System.Drawing.Point((int)(rect.Left + widthDis), (int)(rect.Bottom - heightDis));
        }
        /// <summary>
        /// 坐标转化，从集水井代码中拷贝过来的
        /// 屏幕坐标到revit平面坐标转换
        /// 转化到xoy平面
        /// </summary>
        /// <param name="_uiView"></param>
        /// <param name="screenPoint"></param>
        /// <returns></returns>
        static public XYZ Screen2ViewPlan(UIView _uiView, System.Drawing.Point screenPoint)
        {
            //屏幕坐标
            var rect = _uiView.GetWindowRectangle();
            //屏幕比例
            double sWidht = rect.Right - rect.Left;
            double sHeight = rect.Bottom - rect.Top;

            double widhtScale = (screenPoint.X - rect.Left) / sWidht;
            double heightScale = (rect.Bottom - screenPoint.Y) / sHeight;

            var corners = _uiView.GetZoomCorners();
            XYZ wLeftBottom = corners[0];
            XYZ wRightTop = corners[1];

            double wWidth = wRightTop.X - wLeftBottom.X;
            double wHeight = wRightTop.Y - wLeftBottom.Y;

            double widthDis = wWidth * widhtScale;
            double heightDis = wHeight * heightScale;

            return new XYZ(wLeftBottom.X + widthDis, wLeftBottom.Y + heightDis, 0);
        }

        /// <summary>
        /// revit平面坐标到屏幕坐标转化
        /// 老方法，只考虑了xoy平面
        /// </summary>
        /// <param name="_uiView"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        static public System.Drawing.Point ViewPlan2Screen(UIView _uiView, XYZ point)
        {
            var corners = _uiView.GetZoomCorners();
            XYZ wLeftBottom = corners[0];
            XYZ wRightTop = corners[1];

            double wWidth = wRightTop.X - wLeftBottom.X;
            double wHeight = wRightTop.Y - wLeftBottom.Y;

            double widthScale = (point.X - wLeftBottom.X) / wWidth;
            double heightScale = (point.Y - wLeftBottom.Y) / wHeight;

            //屏幕坐标
            var rect = _uiView.GetWindowRectangle();
            //屏幕比例
            double sWidht = rect.Right - rect.Left;
            double sHeight = rect.Bottom - rect.Top;

            double widthDis = sWidht * widthScale;
            double heightDis = sHeight * heightScale;

            return new System.Drawing.Point((int)(rect.Left + widthDis), (int)(rect.Bottom - heightDis));
        }

        /// <summary>
        /// 获取revit工作区域的屏幕坐标
        /// </summary>
        /// <param name="_uiView"></param>
        /// <param name="bottomLeft"></param>
        /// <param name="topRight"></param>
        static public void GetWorkspaceRect(UIView _uiView, out System.Drawing.Point bottomLeft, out System.Drawing.Point topRight)
        {
            XYZ wLeftBottom, wRightTop;
            GetWorkspaceRect(_uiView, out wLeftBottom, out wRightTop);

            bottomLeft = ViewPlan2Screen(_uiView, wLeftBottom);
            topRight = ViewPlan2Screen(_uiView, wRightTop);
        }
        static void GetWorkspaceRect(UIView _uiView, out XYZ bottomLeft, out XYZ topRight)
        {
            var corners = _uiView.GetZoomCorners();

            bottomLeft = corners[0];
            topRight = corners[1];
        }
    }
}
