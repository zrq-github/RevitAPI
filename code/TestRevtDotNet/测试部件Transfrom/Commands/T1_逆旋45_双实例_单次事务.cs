using RQ.RevitUtils.ExternalEventUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.Test.RevtDotNet.测试部件Transfrom.Commands
{
    internal class T1_逆旋45_双实例_单次事务 : IExternalEventBase
    {
        private TestAssemblyTransfromPlaneModel transfromModel;

        public T1_逆旋45_双实例_单次事务(TestAssemblyTransfromPlaneModel transfromModel)
        {
            this.transfromModel = transfromModel;
        }

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            Element selEle = doc.GetElement(new ElementId(transfromModel.AssemblyId));
            AssemblyInstance assemblyInst = selEle as AssemblyInstance;

            Element selEle2 = doc.GetElement(new ElementId(transfromModel.Assembly2Id));
            AssemblyInstance assemblyInst2 = selEle2 as AssemblyInstance;
            if (assemblyInst == null) return;

            Transaction transaction = new Transaction(doc, nameof(T1_逆旋45_双实例_单次事务));
            transaction.Start();

            //T1_逆旋45_双实例_单次事务.Process(doc, assemblyInst);
            T1_逆旋45_双实例_单次事务.Process(doc, assemblyInst2);


            transaction.Commit();
        }

        public static void Process(Document doc, AssemblyInstance assemblyInstance)
        {
            Element typeId = doc.GetElement(assemblyInstance.GetTypeId());
            AssemblyType assemblyType = typeId as AssemblyType;

            Transform assemblyTransfrom = assemblyInstance.GetTransform();
            Transform transform = Transform.CreateRotation(assemblyTransfrom.BasisZ, Math.PI / 4);
            transform = assemblyTransfrom * transform;
            assemblyInstance.SetTransform(transform);
        }

        public string GetName()
        {
            return nameof(T1_逆旋45_双实例_单次事务);
        }
    }
}
