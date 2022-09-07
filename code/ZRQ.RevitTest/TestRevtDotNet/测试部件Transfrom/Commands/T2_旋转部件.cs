using RQ.RevitUtils.ExternalEventUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.Test.RevtDotNet.测试部件Transfrom.Commands
{
    internal class T2_旋转部件 : IExternalEventBase
    {
        private TestAssemblyTransfromPlaneModel transfromModel;

        public T2_旋转部件(TestAssemblyTransfromPlaneModel transfromModel)
        {
            this.transfromModel = transfromModel;
        }

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            Element selEle = doc.GetElement(new ElementId(transfromModel.AssemblyId));
            AssemblyInstance assemblyInstance = selEle as AssemblyInstance;
            if (assemblyInstance == null) return;

            Transaction transaction = new Transaction(doc, nameof(T2_旋转部件));
            transaction.Start();

            T2_旋转部件.Process(doc, assemblyInstance);

            transaction.Commit();
        }

        public static void Process(Document doc, AssemblyInstance assemblyInstance)
        {
            Transform assemblyTransfrom = assemblyInstance.GetTransform();
            
            //Transform transform = Transform.CreateRotation(assemblyTransfrom.BasisZ, Math.PI / 4);
            //transform = assemblyTransfrom * transform;
            ElementTransformUtils.RotateElement(doc, assemblyInstance.Id, Line.CreateBound(assemblyTransfrom.Origin, assemblyTransfrom.BasisZ), Math.PI / 4);
        }

        public string GetName()
        {
            return nameof(T2_旋转部件);
        }
    }
}
