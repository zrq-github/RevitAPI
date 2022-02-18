using Autodesk.Revit.Attributes;
using RQ.Test.RevtDotNet.测试扩展数据;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.Test.RevtDotNet.测试部件Transfrom.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class T0_ShowWin : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TestAssemblyTransfromPlane testAssemblyTransfromPlane = new TestAssemblyTransfromPlane();
            testAssemblyTransfromPlane.Show();
            return Result.Succeeded;
        }
    }
}
