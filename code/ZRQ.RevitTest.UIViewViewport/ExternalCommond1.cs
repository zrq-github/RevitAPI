using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace ZRQ.RevitTest.UIViewViewport
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ExternalCommond1 : IExternalCommand
    {
        UIApplication UIApp = null;
        Pipe SelPipe { get; set; }

        Viewport Viewport { get; set; }

        ViewSheet ViewSheet { get; set; }

        Autodesk.Revit.DB.View View { get; set; }

        Document Doc { get; set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ShellUtils.Inst.Info("启动控制台");

            UIApp = commandData.Application;
            Document doc = UIApp.ActiveUIDocument.Document;
            this.Doc = doc;

            Element element = doc.GetElement(new ElementId(223791));
            SelPipe = (Pipe)element;
            this.Viewport = doc.GetElement(new ElementId(223709)) as Viewport;
            LocationPoint locationPoint = (this.Viewport.Location as LocationPoint);
            this.ViewSheet = doc.GetElement(new ElementId(223690)) as ViewSheet;

            // 视口里面的三维图形
            this.View = this.Doc.GetElement(this.Viewport.ViewId) as Autodesk.Revit.DB.View;

            XYZ selectPt = null;
            PreviewData previewData = new PreviewData();

            using (DimJIGMng dimJIG = new DimJIGMng(UIApp, previewData))
            {
                dimJIG.DrawJIGAction = DrawJIGAction;
                selectPt = dimJIG.DoJIG();
            }

            return Result.Succeeded;
        }

        private void DrawJIGAction(System.Windows.Forms.Form form, UIView activeUIView, System.Drawing.Point cursorPt, Data4DimBase data)
        {
            ShellUtils.Inst.Info("==================Start DrawJIGAction================");
            ShellUtils.Inst.Info($"{nameof(cursorPt)},x:{cursorPt.X},Y:{cursorPt.Y}");


            ShellUtils.Inst.Info("UIView:======");
            Rectangle rectangle = activeUIView.GetWindowRectangle();
            ShellUtils.Inst.Info($"{nameof(UIView)}Rectangle: \n" +
                $"Rectangle.Bottom: {rectangle.Bottom}\n" +
                $"Rectangle.Right: {rectangle.Right}\n" +
                $"Rectangle.Top: {rectangle.Top}\n" +
                $"Rectangle.Left: {rectangle.Left}\n");

            var corners = activeUIView.GetZoomCorners();
            XYZ wLeftBottom = corners[0];
            XYZ wRightTop = corners[1];
            ShellUtils.Inst.Info($"View.GetZoomCorners: \n" +
                $"LeftBottom: {wLeftBottom} \n" +
                $"RightTop: {wRightTop}\n");

            ShellUtils.Inst.Info("Viewport:======");
            ShellUtils.Inst.Info($"视口相关的一些参数: \n" +
                $"GetBoxCenter: {this.Viewport.GetBoxCenter()} \n" +
                $"GetBoxOutline().MinimumPoint: {this.Viewport.GetBoxOutline().MinimumPoint} \n" +
                $"GetBoxOutline().MaximumPoint: {this.Viewport.GetBoxOutline().MaximumPoint} \n");

            ShellUtils.Inst.Info("View(视口里面的视图):======");
            ShellUtils.Inst.Info($"\nCropBox: \n" +
                $"CropBox.Min: {this.View.CropBox.Min} \n" +
                $"CropBox.Max: {this.View.CropBox.Max} \n" +
                $"Outline: \n" +
                $"Outline.Min: {this.View.Outline.Min} \n" +
                $"Outline.Max: {this.View.Outline.Max} \n" +
                $"Center: {(this.View.Outline.Min + this.View.Outline.Max) / 2}");


            Line fristLine = (SelPipe.Location as LocationCurve).Curve as Line;
            XYZ spt = fristLine.GetEndPoint(0);
            XYZ endPt = fristLine.GetEndPoint(1);
            ShellUtils.Inst.Info($"管道的点: \n" +
                $"起始点: {spt} \n" +
                $"结束点: {endPt}");

            LocationPoint locationPoint = (this.Viewport.Location as LocationPoint);
            Autodesk.Revit.DB.View activeGraphicalView = UIApp.ActiveUIDocument.ActiveGraphicalView;

            XYZ viewSheetOrigin = this.ViewSheet.Origin;
            ShellUtils.Inst.Info($"viewSheetOrigin{viewSheetOrigin}");

            //System.Drawing.Point screenPoint = UIViewTool.ViewPlan2Screen_noActiveView(spt, activeUIView, activeGraphicalView, this.Viewport, this.View, this.ViewSheet);
            //ShellUtils.Inst.Info($"起始点转到屏幕上的点{screenPoint}");

            System.Drawing.Point screenPoint = UIViewTool.ViewPlan2Screen_ActiveView(spt, activeUIView, this.Viewport, this.View, this.ViewSheet);
            ShellUtils.Inst.Info($"起始点转到屏幕上的点{screenPoint}");


            //XYZ cursorPtOnActiveGraphicalView = UIViewTool.Screen2ViewPlan(cursorPt, activeUIView, activeGraphicalView);
            //ShellUtils.Inst.Info($"cursorPt To active view {cursorPtOnActiveGraphicalView}");
            //XYZ cursorPtToXOY = UIViewTool.Screen2ViewPlan(activeUIView, cursorPt);
            //ShellUtils.Inst.Info($" cursorPt To XOY平面 {cursorPtOnActiveGraphicalView}");

            // 画线
            List<System.Drawing.Point> points = new List<System.Drawing.Point>();
            HWTransCommon.Win32DllImport.Win32Window.ScreenToClient(form.Handle, ref screenPoint);
            points.Add(screenPoint);
            HWTransCommon.Win32DllImport.Win32Window.ScreenToClient(form.Handle, ref cursorPt);
            points.Add(cursorPt);

            System.Drawing.Pen drawPen = GetPenColor(UIApp);
            using (System.Drawing.Graphics graphic = form.CreateGraphics())
            {
                graphic.Clear(form.BackColor);
                graphic.DrawLines(drawPen, points.ToArray());
            }

            ShellUtils.Inst.Info("==================End DrawJIGAction================ \n");
        }

        /// <summary>
        /// 根据revit的背景颜色, 初始化引线的默认颜色
        /// </summary>
        /// <param name="uiApp"></param>
        /// <returns></returns>
        /// <remarks>避免在revit是黑色背景的时候, 引线颜色为黑色而开不见</remarks>
        private System.Drawing.Pen GetPenColor(UIApplication uiApp)
        {
#if REVIT2014 || REVIT2015
            // 因为机电不在支持 revit2016前  这里没有做修改
            var view = _view;
            var viewbg = view.GetBackground();
            var bg = viewbg.BackgroundColor;
            if (!(bg.Blue == 255 && bg.Red == 255 && bg.Green == 255))
            {
                _penColor = System.Drawing.Pens.White;
            }
            else
            {
                _penColor = System.Drawing.Pens.Black;
            }
#else
            System.Drawing.Pen penColor = null;
            Autodesk.Revit.DB.Color bg = uiApp.Application.BackgroundColor;
            if (!(bg.Blue == 255 && bg.Red == 255 && bg.Green == 255))
            {//不是白色
                penColor = System.Drawing.Pens.White;
            }
            else
            {
                penColor = System.Drawing.Pens.Black;
            }
            return penColor;
#endif
        }

    }
}