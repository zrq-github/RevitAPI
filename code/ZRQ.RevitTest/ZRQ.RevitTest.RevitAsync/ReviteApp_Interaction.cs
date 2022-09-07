using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitTest.RevitAsync
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true)]
    public class ReviteApp_Interaction : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            Revit.Async.RevitTask.Initialize(application);

            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
