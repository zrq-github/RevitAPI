using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitUtils
{
    /// <summary>
    /// BoundingBoxXYZ Operation class
    /// </summary>
    public static class BoundingBoxXyzUtils
    {
        /// <summary>
        /// 得到BoundingBox底部一圈的CurveLoop
        /// </summary>
        public static CurveLoop GetBottomCurveLoop(BoundingBoxXYZ boxXyz)
        {
            var minpt = GetMin3D(boxXyz);
            var maxpt = GetMax3D(boxXyz);

            var pt2 = new XYZ(maxpt.X, minpt.Y, minpt.Z);
            var pt3 = new XYZ(maxpt.X, maxpt.Y, minpt.Z);
            var pt4 = new XYZ(minpt.X, maxpt.Y, minpt.Z);
            Line line1 = Line.CreateBound(minpt, pt2);
            Line line2 = Line.CreateBound(pt2, pt3);
            Line line3 = Line.CreateBound(pt3, pt4);
            Line line4 = Line.CreateBound(pt4, minpt);
            CurveLoop curveLoop = new CurveLoop();
            curveLoop.Append(line1);
            curveLoop.Append(line2);
            curveLoop.Append(line3);
            curveLoop.Append(line4);

            return curveLoop;
        }

        /// <summary>
        /// 获取BoudingBoxXYZ的中心点
        /// </summary>
        /// <remarks>
        /// zrq: 为什么要使用boxXYZ.Transform?
        /// 因为在某些情况下, 获取到的boxXYZ, 并不是常规的BoundingBoxXYZ,
        /// 它的Transfrom并不位于左下角, 而是在中间, 这样得到的数据可能是错误的.
        /// </remarks>
        public static XYZ GetCenter3D(BoundingBoxXYZ boxXyz)
        {
            return (GetMin3D(boxXyz) + GetMax3D(boxXyz)) / 2;
        }

        /// <summary>
        /// 获取最大点(实际坐标中的位置)
        /// </summary>
        /// <remarks>
        /// zrq: BoudingBoxXYZ的最小点 ≠ 实际坐标 
        /// 例如创建的Solid.
        /// </remarks>
        public static XYZ GetMax3D(BoundingBoxXYZ boxXyz)
        {
            return boxXyz.Transform.OfPoint(boxXyz.Max);
        }

        /// <summary>
        /// 获取最小点(实际坐标中的位置)
        /// </summary>
        /// <remarks>
        /// zrq: BoudingBoxXYZ的最小点 ≠ 实际坐标 
        /// 例如创建的Solid.
        /// </remarks>
        public static XYZ GetMin3D(BoundingBoxXYZ boxXyz)
        {
            return boxXyz.Transform.OfPoint(boxXyz.Min);
        }

        public static BoundingBoxXYZ GetTransformedBoundingBoxXyz(BoundingBoxXYZ boxXyz)
        {
            BoundingBoxXYZ boundingBoxXyz = new BoundingBoxXYZ();
            boundingBoxXyz.Min = GetMin3D(boxXyz);
            boundingBoxXyz.Max = GetMax3D(boxXyz);
            boundingBoxXyz.Transform = Transform.Identity;
            return boundingBoxXyz;
        }

        /// <summary>
        /// 将BoudingBoxXYZ转化成OutLine
        /// </summary>
        /// <remarks>
        /// zrq: 这种多用于 Filter 
        /// </remarks>
        public static Outline GetOutline(BoundingBoxXYZ boxXyz)
        {
            return new Outline(GetMin3D(boxXyz), GetMax3D(boxXyz));
        }

        /// <summary>
        /// 是否相交
        /// </summary>
        /// <remarks>
        /// zrq: 本质上是判断两个Box是否相交, 如果这个写法有问题, 直接改就好了
        /// </remarks>
        public static bool IsIntersect(BoundingBoxXYZ source, BoundingBoxXYZ compare, double tolerance = 0)
        {
            Outline outline = GetOutline(source);
            Outline compareOutline = GetOutline(compare);
            return outline.Intersects(compareOutline, tolerance);
        }
    }
}
