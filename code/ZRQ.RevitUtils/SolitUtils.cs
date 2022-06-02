using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitUtils
{
    internal class SolitUtils
    {
        /// <summary>
        /// 获取Solid的默认设置
        /// </summary>
        /// <returns></returns>
        private static Options GetDefaultOptions()
        {
            Options options = new Options();
            options.ComputeReferences = true;
            options.DetailLevel = Autodesk.Revit.DB.ViewDetailLevel.Fine;
            return options;
        }

        public static Solid GetElementSolid(Element element)
        {
            Options opt = GetDefaultOptions();
            Transform trans = Transform.Identity;

            GeometryElement ge = element.get_Geometry(opt);
            GeometryElement geTransformed = ge.GetTransformed(trans);

            return GetElementSolid(element, opt, trans);
        }

        public static Solid GetElementSolid(Element element, Transform trans)
        {
            Options opt = GetDefaultOptions();
            return GetElementSolid(element, opt, trans);
        }

        public static Solid GetElementSolid(Element element, Options opt, Transform trans)
        {
            GeometryElement ge = element.get_Geometry(opt);
            GeometryElement geTransformed = ge.GetTransformed(trans);
            IEnumerator<GeometryObject> iter = geTransformed.GetEnumerator();
            while (iter.MoveNext())
            {
                Solid solid = iter.Current as Solid;
                if (solid == null)
                {
                    continue;
                }

                FaceArray faces = solid.Faces;
                if (faces.IsEmpty)
                {
                    continue;
                }

                return solid;
            }

            return null;
        }
    }
}
