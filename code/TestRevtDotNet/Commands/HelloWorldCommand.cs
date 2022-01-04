/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/3 17:37:29
 * 文件描述:  
 * 
*************************************************************************************/
using Autodesk.Revit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.Test.RevtDotNet.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class HelloWorldCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var a = Assembly.GetExecutingAssembly();
            var dialog = new TaskDialog("Autodesk Revit");
            dialog.MainContent = $"Hello World from {a.Location} v{a.GetName().Version}";
            dialog.Show();
            return Result.Cancelled;
        }
    }
}