using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using HWTransCommon;
using HWTransCommon.GeometricTool;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitTest.UIViewViewport
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class 画视口中心点 : IExternalCommand
    {
        UIApplication UIApp = null;
        Pipe SelPipe { get; set; }
        Viewport Viewport { get; set; }

        ViewSheet ViewSheet { get; set; }

        Document Doc { get; set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApp = commandData.Application;
            Document doc = UIApp.ActiveUIDocument.Document;
            this.Doc = doc;

            Element element = doc.GetElement(new ElementId(223791));
            SelPipe = (Pipe)element;
            this.Viewport = doc.GetElement(new ElementId(223709)) as Viewport;
            LocationPoint locationPoint = (this.Viewport.Location as LocationPoint);
            this.ViewSheet = doc.GetElement(new ElementId(223690)) as ViewSheet;

            XYZ viewPortCenter = this.Viewport.GetBoxCenter();

            HWTransCommon.HWTransaction.Process(doc, "画视口中心位置", () =>
            {
                XYZ rightDirection = this.ViewSheet.RightDirection;
                XYZ upDirection = this.ViewSheet.UpDirection;
                XYZ viewDirection = this.ViewSheet.ViewDirection;

                Plane plane = Plane.CreateByNormalAndOrigin(viewDirection, this.ViewSheet.Origin);
                SketchPlane sp = SketchPlane.Create(doc, plane);

                XYZ proViewPortCenter = PlaneExtension.Project(plane, viewPortCenter);

                Line rLine = Line.CreateBound(proViewPortCenter, proViewPortCenter + rightDirection * MathTool.mmToRevit(1000));
                Line lLine = Line.CreateBound(proViewPortCenter, proViewPortCenter - rightDirection * MathTool.mmToRevit(1000));
                Line uLine = Line.CreateBound(proViewPortCenter, proViewPortCenter + upDirection * MathTool.mmToRevit(1000));
                Line dLine = Line.CreateBound(proViewPortCenter, proViewPortCenter - upDirection * MathTool.mmToRevit(1000));

                doc.Create.NewDetailCurve(this.ViewSheet, rLine);
                doc.Create.NewDetailCurve(this.ViewSheet, lLine);
                doc.Create.NewDetailCurve(this.ViewSheet, uLine);
                doc.Create.NewDetailCurve(this.ViewSheet, dLine);
            });

            return Result.Succeeded;
        }
    }
}