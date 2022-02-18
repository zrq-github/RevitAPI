/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2021/12/28 17:17:51
 * 文件描述:  
 * 
*************************************************************************************/



using RQ.RevitUtils;
using RQ.RevitUtils.ExternalEventUtility;
using RQ.Test.RevtDotNet;
using RQ.Test.RevtDotNet.Commands;

namespace TestRevtDotNet
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true)]
    internal class RevitApplication : IExternalApplication
    {
        public static string ApplicationId { get; } = "8402F7DD-5F72-4614-B98D-AAFF4F9EE639";

        public Result OnShutdown(UIControlledApplication application)
        {
            CreateRibbonPanel(application);

            return Result.Succeeded;
        }

        private void CreateRibbonPanel(UIControlledApplication application)
        {
            var ribbonPanel = application.CreatePanel("Revit Lookup");
            var pullDownButton = ribbonPanel.AddPullDownButton("Options", "Revit Lookup");
            pullDownButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
            pullDownButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");
            pullDownButton.AddPushButton(typeof(HelloWorldCommand), "Hello World...");
            //pullDownButton.AddPushButton(typeof(T0_ShowWin), "测试扩展数据");
        }

        public Result OnStartup(UIControlledApplication application)
        {
            ExternalEventHandler.CreateExternalEvent();

            // 可停靠窗口注册
            RegisterDockablePane(application);

            return Result.Succeeded;
        }

        private void RegisterDockablePane(UIControlledApplication application)
        {

        }
    }
}