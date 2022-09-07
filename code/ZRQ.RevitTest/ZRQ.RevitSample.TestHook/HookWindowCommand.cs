using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitSample.TestHook
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class HookWindowCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Autodesk.Revit.DB.View view = commandData.Application.ActiveUIDocument.ActiveGraphicalView;

            RevitTask.Initialize(commandData.Application);
            IntPtr hwndRvt = Autodesk.Windows.ComponentManager.ApplicationWindow;

            //Form hookWindow = new Form();
            //hookWindow.Show(new RevitOwnerWindowHandler());


            HookWindow hookWindow = new HookWindow(commandData.Application);
            hookWindow.Topmost = true;
            System.Windows.Interop.WindowInteropHelper x =
                new System.Windows.Interop.WindowInteropHelper(hookWindow);
            x.Owner = Autodesk.Windows.ComponentManager.ApplicationWindow;
            hookWindow.Show();


            return Result.Succeeded;
        }

        public class RevitOwnerWindowHandler : System.Windows.Forms.IWin32Window
        {
            private IntPtr _hwnd = Autodesk.Windows.ComponentManager.ApplicationWindow;

            /// <summary>
            /// 句柄
            /// </summary>
            public IntPtr Handle
            {
                get
                {
                    return this._hwnd;
                }
            }
        }
    }
}