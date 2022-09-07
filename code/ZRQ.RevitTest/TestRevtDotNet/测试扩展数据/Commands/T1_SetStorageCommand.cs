/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/2 21:11:41
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
using System.Windows.Controls.Primitives;

namespace RQ.Test.RevtDotNet.测试扩展数据.Commands
{
    internal class T1_SetStorageCommand : IExternalEventBase
    {
        public T1_SetStorageCommand(TestStorageWinModel testStorageWinModel)
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
            T1StorageData storage_Person = new T1StorageData();
            storage_Person.LastCommand = nameof(T1_SetStorageCommand);

            ExtendStorageTable extendStorageTable = new ExtendStorageTable();
            extendStorageTable.SetT1StorageData(selEle, storage_Person);

            A a = new A();
            extendStorageTable.SetDictionary(selEle, a);


            transaction.Commit();
        }

        public string GetName()
        {
            return nameof(T1_SetStorageCommand);
        }
    }
}