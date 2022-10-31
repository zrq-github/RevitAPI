using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitJD
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class DoThings : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            HW.Tool.Load.Process();
            return Result.Succeeded;
        }
    }
}