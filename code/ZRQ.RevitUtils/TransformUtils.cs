using Autodesk.Revit.DB;

namespace ZRQ.RevitUtils
{
    /// <summary>
    /// transform operation class
    /// </summary>
    public static class TransformUtils
    {
        /// <summary>
        /// 由plane构建transform
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Transform GetTransformByPlane(Plane plane)
        {
            var retTransform = Transform.Identity;
            retTransform.BasisX = plane.XVec;
            retTransform.BasisY = plane.YVec;
            retTransform.BasisZ = plane.Normal;
            retTransform.Origin = plane.Origin;
            return retTransform;
        }
    }
}
