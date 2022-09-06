using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitUtils
{
    /// <summary>
    /// 待完善
    /// </summary>
    internal class View3DUtils
    {
        public static View3D CreateView3D(Document Doc, string view3dName)
        {
            Document doc = Doc;
            //先判断当前document中 是否已经存在名字为 view3dName 的三维视图
            View3D theView = GetView3D(doc, view3dName);
            if (theView != null)
            {
                return theView;
            }

            XYZ xYZ = new XYZ(-1.0, 1.0, -1.0);
            View3D newView3D = NewView3D(doc, xYZ);
            if (newView3D == null)
            {
                return null;
            }
            newView3D.Name = view3dName;
            Parameter param = newView3D.get_Parameter(BuiltInParameter.VIEW_DISCIPLINE);
            param.Set((int)ViewDiscipline.Coordination);
            return newView3D;
        }

        public static View3D GetView3D(Document doc, string view3dName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> all3dViews = collector.OfClass(typeof(View3D)).ToElements();
            foreach (Element ele in all3dViews)
            {
                View3D theView = ele as View3D;
                if (theView.Name.Equals(view3dName))//当前图纸已经存在名为view3dName的三维视图
                {
                    return theView;
                }
            }

            return null;
        }

        public static ElementId GetView3dFamilyTypeId(Document doc)
        {
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc);
            IList<Element> source = filteredElementCollector.OfClass(typeof(ViewFamilyType)).ToElements();
            IEnumerable<ViewFamilyType> source2 =
                from elem in source
                let theViewFamilyType = elem as ViewFamilyType
                where theViewFamilyType.ViewFamily == ViewFamily.ThreeDimensional //102
                select theViewFamilyType;
            if (source2.First<ViewFamilyType>() == null)
            {
                return null;
            }
            return source2.First<ViewFamilyType>().Id;
        }

        public static View3D NewView3D(Document doc, XYZ direction)
        {
            ElementId view3dFamilyTypeId = GetView3dFamilyTypeId(doc);
            if (null == view3dFamilyTypeId)
            {
                return null;
            }
            View3D view3D = View3D.CreateIsometric(doc, view3dFamilyTypeId);
            //if (view3D != null)
            //{
            //    XYZ xYZ = direction * 10.0;
            //    XYZ xYZ2 = new XYZ(direction.X, direction.Y, 0.0);
            //    XYZ xYZ3;
            //    if (xYZ2.IsAlmostEqualTo(XYZ.Zero, HWTransCommon.MathTool.mmToRevit(0.1)))
            //    {
            //        xYZ3 = XYZ.BasisY;
            //    }
            //    else
            //    {
            //        xYZ2 = RotateTo(xYZ2, -Math.PI / 2, XYZ.BasisZ);
            //        xYZ3 = RotateTo(direction, Math.PI / 2, xYZ2);
            //    }
            //    ViewOrientation3D orientation = new ViewOrientation3D(xYZ, xYZ3, direction);
            //    view3D.SetOrientation(orientation);
            //}
            return view3D;
        }
    }
}
