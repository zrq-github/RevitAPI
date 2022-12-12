using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitUtils
{
    /// <summary>
    /// 元素筛选工具
    /// </summary>
    public class ElementFilterUtils
    {
        /// <summary>
        /// 根据扩展数据的GUID进行筛选
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public static List<Element> GetStorageFilter(Document doc, string storageGuid)
        {
            FilteredElementCollector ele = new(doc);
            Guid guid = new(storageGuid);
            ExtensibleStorageFilter storageFilter = new(guid);
            List<Element> eles = ele.WherePasses(storageFilter).ToList();
            return eles;
        }
    }
}
