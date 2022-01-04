/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/4 15:38:05
 * 文件描述:  
 * 
*************************************************************************************/
using RQ.RevitUtils.ExternalEventUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.Test.RevtDotNet.测试扩展数据.Commands
{
    internal class T2_ClassUpdateCommand : IExternalEventBase
    {
        public T2_ClassUpdateCommand(TestStorageWinModel testStorageWinModel)
        {
            TestStorageWinModel = testStorageWinModel;
        }

        public TestStorageWinModel TestStorageWinModel { get; }

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            Element selEle = doc.GetElement(new ElementId(TestStorageWinModel.TestElementId));

            Transaction transaction = new Transaction(doc, nameof(T2_ClassUpdateCommand));
            transaction.Start();

            T2StorageData storage_Person = null;
            ExtendStorageTable extendStorageTable = new ExtendStorageTable();
            storage_Person = extendStorageTable.GetT2StorageData(selEle);

            transaction.Commit();
        }

        public string GetName()
        {
            return nameof(T2_ClassUpdateCommand);
        }
    }
}