using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitTest.RevitAsync
{
    public static class TestRevitTask
    {
        public static Task DoTastRotation()
        {
            // 旋转管道45°.
            Task task = Revit.Async.RevitTask.RunAsync(app =>
            {
                Document doc = app.ActiveUIDocument.Document;
                Element ele = doc.GetElement(new ElementId(231173));

                Transform transform = Transform.Identity;
                Transform.CreateRotation(new XYZ(0, 0, 1), Math.PI / 4);

                using (Transaction trans = new Transaction(doc, "旋转管道45°"))
                {
                    trans.Start();
                    Pipe pipe = ele as Pipe;
                    Line locationLine = (pipe.Location as LocationCurve).Curve as Line;
                    XYZ startXYZ = locationLine.GetEndPoint(0);
                    pipe.Location.Rotate(Line.CreateUnbound(startXYZ, new XYZ(0, 0, 1)), Math.PI / 4);
                    trans.Commit();
                }
            });

            return task;
        }

        public static Task DoTaskMove()
        {
            Task task = Revit.Async.RevitTask.RunAsync(app =>
            {
                Document doc = app.ActiveUIDocument.Document;
                Element ele = doc.GetElement(new ElementId(231173));

                Transform transform = Transform.Identity;
                Transform.CreateRotation(new XYZ(0, 0, 1), Math.PI / 4);

                using (Transaction trans = new Transaction(doc, "移动"))
                {
                    trans.Start();
                    Pipe pipe = ele as Pipe;
                    Line locationLine = (pipe.Location as LocationCurve).Curve as Line;
                    XYZ startXYZ = locationLine.GetEndPoint(0);
                    pipe.Location.Move(new XYZ(0.5, 0, 0));
                    trans.Commit();
                }
            });

            return task;
        }
    }
}
