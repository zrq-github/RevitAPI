using Autodesk.Revit.DB;

namespace HWTransCommon.HWRevit
{
    public class FilterStringRuleUtil
    {
        /// <summary>
        /// FilterStringRule 的构造的版本适配
        /// 四参数含有caseSensitive的在2022弃用,在2023删除用不含有caseSensitive的代替
        /// </summary>
        /// <param name="valueProvider"></param>
        /// <param name="evaluator"></param>
        /// <param name="ruleString"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        public static FilterStringRule Create(FilterableValueProvider valueProvider, FilterStringRuleEvaluator evaluator, string ruleString, bool caseSensitive = true)
        {
            FilterStringRule filterStringRule = null;

#if REVIT2016 || REVIT2017 || REVIT2018 || REVIT2019 || REVIT2020 || REVIT2021
            filterStringRule = new FilterStringRule(valueProvider, evaluator, ruleString, caseSensitive);
#else
            filterStringRule = new FilterStringRule(valueProvider, evaluator, ruleString);
#endif
            return filterStringRule;
        }
    }
}
