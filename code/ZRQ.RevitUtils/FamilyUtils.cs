using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitUtils
{
    /// <summary>
    /// 对族文件的一系列操作
    /// </summary>
    /// <remarks>
    /// 因为族
    /// </remarks>
    public class FamilyUtils
    {
        /// <summary>
        /// 获取族
        /// </summary>
        /// <param name="doc">Document</param>
        /// <param name="familyName">FmailyName</param>
        /// <returns>
        /// null: 文档中不存在该族
        /// </returns>
        public static Family GetFamily(Document doc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Family));

            foreach (Family fm in collector)
            {
                if (fm.Name.Equals(familyName.Trim()))
                {
                    return fm;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取系统族
        /// </summary>
        /// <param name="doc">Document</param>
        /// <param name="familyName">FmailyName</param>
        /// <returns>
        /// null: 文档中不存在该系统族
        /// </returns>
        public static ElementType GetSystemFamily(Document doc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(ElementType));

            foreach (ElementType rt in collector)
            {
                if (rt.Name.Equals(familyName.Trim()))
                {
                    return rt;
                }
            }

            return null;
        }

        /// <summary>
        /// 是否存在系统族
        /// </summary>
        /// <remarks>
        /// 系统族也是内置族（暂且称呼为族）
        /// </remarks>
        public static bool IsExistSystemFamily(Document doc, string familyName)
        {
            ElementType elementType = GetSystemFamily(doc, familyName);
            if (elementType == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 在文档中是否存在族
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="familyName"></param>
        /// <returns></returns>
        public static bool IsExistFamily(Document doc, string familyName)
        {
            Family family = GetFamily(doc, familyName);
            if (family == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 得到所有的族类型
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="familyID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">familyID不是族ID</exception>
        public static List<FamilySymbol> GetFamilySymbols(Document doc, ElementId familyID)
        {
            Family family = doc.GetElement(familyID) as Family;
            if (family == null)
            {
                throw new ArgumentNullException(nameof(family), $"{nameof(familyID)} is not Family's ID");
            }
            return GetFamilySymbols(doc, family);
        }

        /// <summary>
        /// 得到所有的族类型
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        public static List<FamilySymbol> GetFamilySymbols(Document doc, Family family)
        {
            List<FamilySymbol> familySymbols = new List<FamilySymbol>();

            List<ElementId> symbolIds = family.GetFamilySymbolIds().ToList();
            foreach (var symbolid in symbolIds)
            {
                FamilySymbol fs = doc.GetElement(symbolid) as FamilySymbol;
                if (fs == null)
                {
                    continue;
                }
                familySymbols.Add(fs);
            }

            return familySymbols;
        }

    }
}
