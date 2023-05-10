using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitUtils
{
    public class IndependentTagUtils
    {
        /// <summary>
        /// 用于从一组IndependentTag中找指定TaggedLocalElementId的IndependentTag
        /// 适配IndependentTag取TaggedLocalElementId的接口 independentTag.TaggedLocalElementId在2022弃用2023删除,用independentTag.GetTaggedLocalElementIds()代替
        /// </summary>
        /// <param name="sourceElements"></param>
        /// <param name="taggedLocalElementId"></param>
        /// <returns></returns>
        public static List<IndependentTag> FindIndependentTag(List<IndependentTag> sourceElements, ElementId taggedLocalElementId)
        {
            List<IndependentTag> independentTags = new List<IndependentTag>();
#if REVIT2016 || REVIT2017 || REVIT2018 || REVIT2019 || REVIT2020 || REVIT2021
                independentTags = sourceElements.FindAll(p => ((p as IndependentTag).TaggedLocalElementId.Equals(taggedLocalElementId)));
#else
            independentTags = sourceElements.FindAll(p => p.GetTaggedLocalElementIds().Contains(TaggedLocalElementId)).ToList();
#endif
            return independentTags;
        }
    }
}
