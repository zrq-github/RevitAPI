using Autodesk.Revit.DB;

namespace ZRQ.RevitUtils
{
    /// <summary>
    /// 
    /// </summary>
    public static class GeometryUtils
    {
        /// <summary>
        /// 得到元素的几何信息
        /// </summary>
        /// <param name="options">
        /// null: 默认行为
        /// </param>
        public static GeometryElement GetGeometryElement(Element element, Options options = null)
        {
            if (null == options)
            {
                options = new Options();
            }
            return element.get_Geometry(options);
        }

        /// <summary>
        /// GetGeometryObjects
        /// </summary>
        /// <param name="options">
        /// null: 默认设置
        /// </param>
        /// <returns>
        /// zrq
        /// </returns>
        public static IEnumerator<GeometryObject> GetGeometryObjects(Element element, Options options = null)
        {
            GeometryElement geometryElement = GetGeometryElement(element, options);
            return geometryElement.GetEnumerator();
        }
    }
}
