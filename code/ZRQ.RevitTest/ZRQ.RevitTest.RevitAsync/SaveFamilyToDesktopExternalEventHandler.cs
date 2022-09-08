using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.Async.ExternalEvents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitTest.RevitAsync
{
    public class SaveFamilyToDesktopExternalEventHandler :
        SyncGenericExternalEventHandler<Family, string>
    {
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            return "SaveFamilyToDesktopExternalEventHandler";
        }

        protected override string Handle(UIApplication app, Family parameter)
        {
            //在这里写同步代码逻辑
            var document = parameter.Document;
            var familyDocument = document.EditFamily(parameter);
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var path = Path.Combine(desktop, $"{parameter.Name}.rfa");
            familyDocument.SaveAs(path, new SaveAsOptions { OverwriteExistingFile = true });
            return path;
        }
    }
}
