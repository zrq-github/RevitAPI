using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using HWLoadBinDLL;
using Nice3point.Revit.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RevitJD
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true)]
    public class Application : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            AppDomain.CurrentDomain.AssemblyResolve += HW.LoadBinDLL.HWLoadBinDLL.OnAssemblyResolve;

            CreateRibbonPanel(application);
            ModelessWindowHandle.SetHandler(application.MainWindowHandle);
            ExternalExecutor.CreateExternalEvent();
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private static void CreateRibbonPanel(UIControlledApplication application)
        {
            var ribbonPanel = application.CreatePanel("RevitJD");
            var pullDownButton = ribbonPanel.AddPullDownButton("Options", "RevitJD");
            pullDownButton.AddPushButton(typeof(DoThings), "DoThings");
        }
    }
}