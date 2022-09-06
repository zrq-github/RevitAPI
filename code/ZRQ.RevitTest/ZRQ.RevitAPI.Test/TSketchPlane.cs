using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitAPI.Test
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class TSketchPlane : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;


            Transaction transaction = new Transaction(doc, "TSketchPlane");
            transaction.Start();

            try
            {
                Plane plane = Plane.CreateByNormalAndOrigin(new XYZ(0, 0, 1), new XYZ(0, 0, 0));
                SketchPlane sketchPlane = SketchPlane.Create(doc, plane);
                bool IsSuitableForModelElements = sketchPlane.IsSuitableForModelElements;
                uiApp.ActiveUIDocument.ActiveView.SketchPlane = sketchPlane;
                uiApp.ActiveUIDocument.ActiveView.ShowActiveWorkPlane();
            }
            catch
            {

            }
            finally
            {

            }

            transaction.Commit();
            return Result.Succeeded;
        }
    }
}