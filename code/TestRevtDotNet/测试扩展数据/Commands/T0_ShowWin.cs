/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/2 21:42:53
 * 文件描述:  
 * 
*************************************************************************************/
using Autodesk.Revit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.Test.RevtDotNet.测试扩展数据.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class T0_ShowWin : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TextStorageWin textStorageWin = new TextStorageWin();
            textStorageWin.Show();
            return Result.Succeeded;
        }
    }
}