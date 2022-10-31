using Autodesk.Revit.UI;
using HWLoadBinDLL;
using Nice3point.Revit.Extensions;
using System;
using System.Reflection;

namespace RevitCC
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
            var ribbonPanel = application.CreatePanel("RevitCC");
            var pullDownButton = ribbonPanel.AddPullDownButton("Options", "RevitCC");
            pullDownButton.AddPushButton(typeof(DoThings), "DoThings");
        }
    }
}
