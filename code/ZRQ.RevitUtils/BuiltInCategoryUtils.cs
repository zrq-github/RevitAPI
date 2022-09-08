using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWTransCommon.HWRevit
{
    /// <summary>
    /// revit BuiltInCategory 的一些操作
    /// </summary>
    /// <remarks>
    /// 针对 BuiltInCategory的一些通用方法
    /// </remarks>
    public static class BuiltInCategoryUtils
    {
        /// <summary>
        /// BuiltInCategory 对应的中文名
        /// </summary>
        /// <remarks>
        /// zrq: 此函数对应应该是 revit(中文) 所对应的实际名字, 请勿自定义名字对应表
        /// 此API在2020就出现, 为什么2020依旧用字典. 2020 风口叫风口末端, 以上版本都叫风口了
        /// </remarks>
        /// <returns>
        /// string: 如果找到对应的名字, 就返回, 没有就是直接返回枚举的 ToString
        /// </returns>
        public static string ToZhCh(Autodesk.Revit.DB.BuiltInCategory builtInCategory)
        {
#if REVIT2016 || REVIT2017 || REVIT2018 || REVIT2019 || REVIT2020
            if (BuiltInCategory_Zh_CN.ContainsKey(builtInCategory))
            {
                return BuiltInCategory_Zh_CN[builtInCategory];
            }
            return builtInCategory.ToString();
#else
            return LabelUtils.GetLabelFor(builtInCategory);
#endif
        }

#if REVIT2016 || REVIT2017 || REVIT2018 || REVIT2019 || REVIT2020
        private static Dictionary<BuiltInCategory, string> BuiltInCategory_Zh_CN = new Dictionary<BuiltInCategory, string>()
        {
            {BuiltInCategory.OST_CableTray,"电缆桥架"},
            {BuiltInCategory.OST_CableTrayFitting,"电缆桥架配件"},
            {BuiltInCategory.OST_Ceilings,"天花板"},
            {BuiltInCategory.OST_Conduit,"线管"},
            {BuiltInCategory.OST_ConduitFitting,"线管配件"},
            {BuiltInCategory.OST_DuctAccessory,"风管附件"},
            {BuiltInCategory.OST_DuctCurves,"风管"},
            {BuiltInCategory.OST_DuctFitting,"风管管件"},
            {BuiltInCategory.OST_DuctInsulations,"风管隔热层"},
            {BuiltInCategory.OST_DuctLinings,"风管内衬"},
            {BuiltInCategory.OST_DuctTerminal,"风口"},
            {BuiltInCategory.OST_FlexDuctCurves,"软风管"},
            {BuiltInCategory.OST_FlexPipeCurves,"软管"},
            {BuiltInCategory.OST_Floors,"楼板"},
            {BuiltInCategory.OST_MechanicalEquipment,"机械设备"},
            {BuiltInCategory.OST_PipeAccessory,"管道附件"},
            {BuiltInCategory.OST_PipeCurves,"管道"},
            {BuiltInCategory.OST_PipeFitting,"管件"},
            {BuiltInCategory.OST_PipeInsulations,"管道隔热层"},
            {BuiltInCategory.OST_Sprinklers,"喷头"},
            {BuiltInCategory.OST_StructuralFraming,"结构框架"},
        };
#endif
    }
}
