using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitAPI.Test
{
    [Transaction(TransactionMode.Manual)]
    public class T_管件附着测试 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;

            // 测试的风管
            ElementId ductId = new ElementId(379476);
            Element ductEle = doc.GetElement(ductId);

            // 测试的风口
            ElementId ductOpentingId = new ElementId(379477);
            FamilyInstance familyInstance = doc.GetElement(ductOpentingId) as FamilyInstance;


            Transaction trans = new Transaction(doc, "管件附着测试");
            trans.Start();

            Transaction trans1 = new Transaction(doc, "管件附着测试aa");


            ConnectorSet conSet = familyInstance.MEPModel.ConnectorManager.Connectors;
            ConnectorSetIterator itor = conSet.ForwardIterator();
            itor.MoveNext();
            Connector connect = itor.Current as Connector;
            Connector refcon = FindRefConnector(connect, true);
            if (refcon != null)
            {
                connect.DisconnectFrom(refcon);//先断开连接
            }
            if (!connect.IsConnected)
            {
                XYZ oldDir_X = connect.CoordinateSystem.BasisX;
                XYZ oldDir_Y = connect.CoordinateSystem.BasisY;
                XYZ oldDir_Z = connect.CoordinateSystem.BasisZ;

                MechanicalUtils.ConnectAirTerminalOnDuct(doc, familyInstance.Id, ductEle.Id);



                XYZ newDir_X = connect.CoordinateSystem.BasisX;

                if (!newDir_X.IsAlmostEqualTo(oldDir_X, 0.1))
                {
                    double angel = newDir_X.AngleOnPlaneTo(oldDir_X, connect.CoordinateSystem.BasisZ);
                    Line lineAxis = Line.CreateUnbound(connect.CoordinateSystem.Origin, connect.CoordinateSystem.BasisZ);
                    connect.CoordinateSystem.BasisX = oldDir_X;
                    connect.CoordinateSystem.BasisY = oldDir_Y;
                    connect.CoordinateSystem.BasisZ = oldDir_Z;
                    doc.Regenerate();
                    //trans1.Start();

                    ElementTransformUtils.RotateElement(doc, familyInstance.Id, lineAxis, Math.PI * 2);

                    ElementTransformUtils.MoveElement(doc, ductId, new XYZ(0, 1, 0));
                    ElementTransformUtils.MoveElement(doc, ductId, -new XYZ(0, 1, 0));


                    //trans1.Commit();
                    trans.Commit();
                }
            }



            return Result.Succeeded;
        }

        /// <summary>
        /// 找到是附着类型的 风口或者是管件
        /// </summary>
        /// <param name="duct"></param>
        /// <returns></returns>
        public static List<FamilyInstance> GetDuctOpening(Duct duct)
        {
            Document doc = duct.Document;
            List<FamilyInstance> listfitting = new List<FamilyInstance>();

            Category category_DuctTerminal = Category.GetCategory(doc, BuiltInCategory.OST_DuctTerminal);
            Category category_DuctFitting = Category.GetCategory(doc, BuiltInCategory.OST_DuctFitting);

            ConnectorSet cons = duct.ConnectorManager.Connectors;
            foreach (Connector con in cons)
            {
                if (con.ConnectorType != ConnectorType.Curve)
                    continue;

                Connector refcon = FindRefConnector(con, true);
                if (refcon != null)
                {
                    Category category = refcon.Owner.Category;
                    if (category.Id == category_DuctTerminal.Id
                        || category.Id == category_DuctFitting.Id)
                    {
                        listfitting.Add(refcon.Owner as FamilyInstance);
                    }
                }
            }

            return listfitting;
        }
        public static List<FamilyInstance> GetDuctFitting(Duct duct, bool bNeedSAndECon = false)
        {
            Document doc = duct.Document;
            double _1mm = 1 / 304.8;
            List<FamilyInstance> listfitting = new List<FamilyInstance>();
            try
            {
                ConnectorSet cons = duct.ConnectorManager.Connectors;
                ConnectorSetIterator itor = cons.ForwardIterator();
                while (itor.MoveNext())
                {//风口侧面连接，在连接器中有体现
                    Connector con = itor.Current as Connector;
                    if (!bNeedSAndECon && con.ConnectorType != ConnectorType.Curve)
                    {
                        continue;
                    }

                    Connector refcon = FindRefConnector(con, true);
                    if (refcon != null)
                    {
                        listfitting.Add(refcon.Owner as FamilyInstance);
                    }
                }

                //第二步，找到直接连接的风口
                BoundingBoxXYZ box = duct.get_BoundingBox(doc.ActiveView);
                XYZ min = box.Min;
                XYZ max = box.Max;
                XYZ dir = (max - min).Normalize();
                min -= _1mm * dir;
                max += _1mm * dir;
                Outline outline = new Outline(min, max);
                FilteredElementCollector coll = new FilteredElementCollector(doc);
                coll.OfClass(typeof(FamilyInstance));
                coll.OfCategory(BuiltInCategory.OST_DuctTerminal);//风口
                coll.WherePasses(new BoundingBoxIntersectsFilter(outline));
                foreach (FamilyInstance item in coll.ToElements())
                {
                    if (listfitting.FindIndex(p => p.Id.Equals(item.Id)) != -1)
                    {
                        continue;
                    }
                    if (!IsConnectToDuct(item, duct))
                    {
                        continue;
                    }
                    listfitting.Add(item);
                }
            }
            catch (Exception)
            {

            }
            return listfitting;
        }
        /// <summary>
        /// 得到与给定连接器相连接的另一个连接器
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="ingroeDist">不判断距离，只判断是否相连。</param>
        /// <returns></returns>
        private static Connector FindRefConnector(Connector connector, bool ingroeDist = false)
        {
            List<Connector> retList = new List<Connector>();
            if (connector != null && connector.IsConnected)
            {
                ConnectorSet linkedConnectors = connector.AllRefs;
                foreach (Connector linkedConnector in linkedConnectors)
                {
                    if (linkedConnector.ConnectorType != ConnectorType.Logical)
                    {
                        if (connector.Owner.Id != linkedConnector.Owner.Id && linkedConnector.IsConnectedTo(connector))
                        {
                            bool isLinked = true;
                            if (!ingroeDist)
                            {
                                try
                                {
                                    isLinked = connector.Origin.DistanceTo(linkedConnector.Origin) < 0.03;
                                }
                                catch (Exception)
                                {
                                    isLinked = false;
                                }
                            }
                            if (isLinked)
                            {
                                retList.Add(linkedConnector);
                            }
                        }
                    }
                }
            }
            if (retList.Count > 1)
            {
            }
            if (retList.Count > 0)
            {
                return retList[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判断管件是否和风管相连
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectToDuct(FamilyInstance fi, Duct duct)
        {
            try
            {
                if (fi.Host != null && fi.Host.Id.Equals(duct.Id))
                {//依附关系
                    return true;
                }

                ConnectorSetIterator conItor = fi.MEPModel.ConnectorManager.Connectors.ForwardIterator();
                while (conItor.MoveNext())
                {//连接关系
                    Connector con = conItor.Current as Connector;
                    if (!con.IsConnected)
                    {
                        continue;
                    }

                    ConnectorSet refs = con.AllRefs;//找到所有连接器连接的连接器
                    ConnectorSetIterator refItor = refs.ForwardIterator();
                    while (refItor.MoveNext())
                    {
                        Connector conTmp = refItor.Current as Connector;
                        if (null == conTmp || !(conTmp.Owner is Duct) ||
                            conTmp.Owner.UniqueId == con.Owner.UniqueId)
                        {
                            continue;
                        }
                        if (!duct.Id.Equals(conTmp.Owner.Id))
                        {
                            continue;
                        }
                        return true;
                    }
                }

                ConnectorSet cons = duct.ConnectorManager.Connectors;
                ConnectorSetIterator itor = cons.ForwardIterator();
                while (itor.MoveNext())
                {//附属配件关系
                    Connector con = itor.Current as Connector;

                    Connector refcon = FindRefConnector(con, true);
                    if (refcon != null && refcon.Owner.Id.Equals(fi.Id))
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}
