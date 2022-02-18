using RQ.RevitUtils.ExternalEventUtility;
using RQ.Test.RevtDotNet.测试扩展数据;
using RQ.Test.RevtDotNet.测试扩展数据.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.Test.RevtDotNet.测试部件Transfrom.Commands
{
    internal class T1_逆时针旋转45 : IExternalEventBase
    {
        private TestAssemblyTransfromPlaneModel transfromModel;

        public T1_逆时针旋转45(TestAssemblyTransfromPlaneModel transfromModel)
        {
            this.transfromModel = transfromModel;
        }

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            Element selEle = doc.GetElement(new ElementId(transfromModel.AssemblyId));
            AssemblyInstance assemblyInstance = selEle as AssemblyInstance;
            if (assemblyInstance == null) return;

            Transaction transaction = new Transaction(doc, nameof(T1_逆时针旋转45));
            transaction.Start();

            T1_逆时针旋转45.Process(doc, assemblyInstance);

            transaction.Commit();
        }

        public static void Process(Document doc, AssemblyInstance assemblyInstance)
        {
            Transform assemblyTransfrom = assemblyInstance.GetTransform();
            Transform transform = Transform.CreateRotation(assemblyTransfrom.BasisZ, Math.PI / 4);
            transform = assemblyTransfrom * transform;
            assemblyInstance.SetTransform(transform);
        }

        public string GetName()
        {
            return nameof(T1_逆时针旋转45);
        }
    }
}
