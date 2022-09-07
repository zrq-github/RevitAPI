using Autodesk.Revit.DB.ExtensibleStorage;
using RQ.RevitUtils.ExternalEventUtility;
using RQ.Test.RevtDotNet.测试扩展数据.Commands;
using System.Diagnostics;

namespace RQ.Test.RevtDotNet.测试扩展数据
{
    internal class T1_ClearStorageCommand : IExternalEventBase
    {
        public T1_ClearStorageCommand(TestStorageWinModel testStorageWinModel)
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

            ExtendStorageTable extendStorageTable = new ExtendStorageTable();
            Schema schema = Schema.Lookup(extendStorageTable.SchemaGuid);
            if (schema != null)
                doc.EraseSchemaAndAllEntities(schema);

            transaction.Commit();
        }

        public string GetName()
        {
            return nameof(T1_ClearStorageCommand);
        }
    }
}