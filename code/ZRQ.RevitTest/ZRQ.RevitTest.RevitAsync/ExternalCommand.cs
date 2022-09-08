using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZRQ.RevitTest.RevitAsync
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ExternalCommand : IExternalCommand
    {
        public static ExternalEvent SomeEvent { get; set; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Revit.Async.RevitTask.Initialize(commandData.Application);
            SomeEvent = ExternalEvent.Create(new ExternalEventHandler());
            //打开非模态窗体
            var window = new MyWindow();
            window.Show();
            return Result.Succeeded;
        }
    }
}