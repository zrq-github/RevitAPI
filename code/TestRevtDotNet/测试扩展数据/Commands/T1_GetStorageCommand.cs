/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/2 21:15:09
 * 文件描述:  
 * 
*************************************************************************************/
using Autodesk.Revit.Attributes;
using RQ.RevitUtils.ExternalEventUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.Test.RevtDotNet.测试扩展数据.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class T1_GetStorageCommand : IExternalEventBase
    {
        public T1_GetStorageCommand(TestStorageWinModel testStorageWinModel)
        {
            TestStorageWinModel = testStorageWinModel;
        }

        public TestStorageWinModel TestStorageWinModel { get; }

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            Element selEle = doc.GetElement(new ElementId(TestStorageWinModel.TestElementId));

            Transaction transaction = new Transaction(doc, nameof(T1_SetStorageCommand));
            transaction.Start();

            T1StorageData storage_Person = null;
            ExtendStorageTable extendStorageTable = new ExtendStorageTable();
            storage_Person = extendStorageTable.GetStoragePerson(selEle);

            transaction.Commit();
        }

        public string GetName()
        {
            return nameof(T1_GetStorageCommand);
        }
    }
}