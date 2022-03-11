using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZRQ.RevitUtils
{
    /// <summary>
    /// 族加载模块
    /// </summary>
    /// <remarks>
    /// 从rvt rte 中加载 族/系统族
    /// 从文件加载族是一系列的操作，
    /// 对于普通的族文件，直接打开族后就可以加载了
    /// 对于系统的族文件，需要打开 rte/rft 文件后，再次进行
    /// </remarks>
    public class FamilyLoadUtils
    {
        /*
         * 加载参数有点多，后续再整理吧
         */

        public static FamilySymbol LoadFamily(Document doc, string familySymbolName, string familyFilePath)
        {
            MyFamilyLoadOptions loadOptions = new MyFamilyLoadOptions();
            doc.LoadFamily(familyFilePath, loadOptions, out Family family);
            return null;
        }
    }

    public class MyFamilyLoadOptions : IFamilyLoadOptions
    {
        /// <summary>
        /// 载入族过程中的设置
        /// </summary>
        /// <param name="familyInUse">族是否被使用</param>
        /// <param name="overwriteParameterValues">覆盖参数</param>
        /// <returns></returns>
        public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
        {
            overwriteParameterValues = true;
            return true;
        }

        /// <summary>
        /// 载入族过程中的设置
        /// </summary>
        /// <param name="sharedFamily"></param>
        /// <param name="familyInUse"></param>
        /// <param name="source"></param>
        /// <param name="overwriteParameterValues"></param>
        /// <returns></returns>
        public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
        {
            source = FamilySource.Family;
            overwriteParameterValues = true;
            return true;
        }
    }
}
