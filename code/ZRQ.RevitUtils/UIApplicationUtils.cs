using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitUtils
{
    public class UIApplicationUtils
    {
        public static UIApplication GetUiApplication(UIControlledApplication uiControlApp)
        {
            var flag = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod;
            var uiApp = (Autodesk.Revit.UI.UIApplication)uiControlApp.GetType().InvokeMember("getUIApplication", flag, Type.DefaultBinder, uiControlApp, null);
            return uiApp;
        }
    }
}
