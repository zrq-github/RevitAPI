using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitUtils
{
    /// <summary>
    /// 明细表的操作
    /// </summary>
    internal class ViewScheduleUtils
    {
        public static void AddField(ViewSchedule view, string fieldName, int index)
        {
            ScheduleDefinition scheduleDefinition = view.Definition;
            FilteredElementCollector collector = new FilteredElementCollector(view.Document);
            SharedParameterElement sharedParameter = collector.OfClass(typeof(SharedParameterElement)).First(element => element.Name == fieldName) as SharedParameterElement;
            if (sharedParameter != null && scheduleDefinition.GetFieldOrder().Select(scheduleDefinition.GetField).Count(field => field.ParameterId == sharedParameter.Id) <= 0)
            {
                ScheduleField scheduleField = scheduleDefinition.AddField(ScheduleFieldType.Instance, sharedParameter.Id);
                var fields = scheduleDefinition.GetFieldOrder();
                fields.Remove(scheduleField.FieldId);
                fields.Insert(index, scheduleField.FieldId);
                scheduleDefinition.SetFieldOrder(fields);
            }
        }

        /// <summary>
        /// 复制明细表
        /// </summary>
        public static void CopyViewSchedule(ViewSchedule des, ViewSchedule src)
        {
            ScheduleCopyFields(des, src);
            ScheduleCopyFilter(des, src);
            ScheduleCopySorting(des, src);
            ScheduleCopyAppearance(des, src);
        }

        /// <summary>
        /// 创建一个属于部件的明细表
        /// </summary>
        /// <returns></returns>
        public static ViewSchedule CreateViewSchedule(Document document, ElementId assemblyInstanceId, ViewSchedule detailTemplate, string name = null)
        {
            var viewSchedule = AssemblyViewUtils.CreateSingleCategorySchedule(document, assemblyInstanceId, detailTemplate.Definition.CategoryId);
            viewSchedule.Name = !string.IsNullOrWhiteSpace(name) ? name : detailTemplate.Name;
            CopyViewSchedule(viewSchedule, detailTemplate);
            return viewSchedule;
        }

        /// <summary>
        /// 找到字段
        /// </summary>
        /// <remarks>
        /// 找到字段，通过比较参数的ID
        /// </remarks>
        public static ScheduleField FindField(List<ScheduleField> newFields, ScheduleField field)
        {
            ElementId paramId;
            if (field.HasSchedulableField)
            {
                paramId = field.GetSchedulableField().ParameterId;
            }
            else
            {
                paramId = field.ParameterId;
            }
            return newFields.FirstOrDefault(f => f.ParameterId == paramId || f.HasSchedulableField && f.GetSchedulableField().ParameterId == paramId);

        }

        /// <summary>
        /// 复制明细表的外观设计
        /// </summary>
        public static void ScheduleCopyAppearance(ViewSchedule des, ViewSchedule src)
        {
            des.BodyTextTypeId = src.BodyTextTypeId;
            des.BodyTextTypeId = src.BodyTextTypeId;
            des.HeaderTextTypeId = src.HeaderTextTypeId;
            des.TitleTextTypeId = src.TitleTextTypeId;
        }

        /// <summary>
        /// 复制明细表的字段
        /// </summary>
        public static void ScheduleCopyFields(ViewSchedule des, ViewSchedule src)
        {
            ScheduleDefinition srcDefinition = src.Definition;
            ScheduleDefinition decDefinition = des.Definition;
            decDefinition.ClearFields();
            List<ScheduleField> srcFields = srcDefinition.GetFieldOrder().Select(srcDefinition.GetField).ToList();

            foreach (ScheduleField srcField in srcFields)
            {
                if (srcField.IsCalculatedField || (!srcField.HasSchedulableField && !IsValid(srcField.ParameterId)))
                    continue;
                ScheduleField desField = decDefinition.AddField(srcField.FieldType, srcField.ParameterId);
                desField.ColumnHeading = srcField.ColumnHeading;
                desField.GridColumnWidth = srcField.GridColumnWidth;
                desField.HeadingOrientation = srcField.HeadingOrientation;
                desField.HorizontalAlignment = srcField.HorizontalAlignment;
                desField.IsHidden = srcField.IsHidden;
                desField.SheetColumnWidth = srcField.SheetColumnWidth;
                desField.SetFormatOptions(srcField.GetFormatOptions());
                desField.SetStyle(srcField.GetStyle());
            }
            List<ScheduleField> newFields = decDefinition.GetFieldOrder().Select(decDefinition.GetField).ToList();
            foreach (ScheduleField item2 in srcFields)
            {
                if (item2.PercentageBy != ScheduleFieldId.InvalidScheduleFieldId)
                {
                    ScheduleField val3 = srcDefinition.GetField(item2.PercentageBy);
                    if (!val3.IsCalculatedField && (val3.HasSchedulableField || IsValid(val3.ParameterId)))
                    {
                        ScheduleField val4 = FindField(newFields, item2);
                        ScheduleField val5 = FindField(newFields, val3);
                        if (val4 != null && val5 != null)
                        {
                            val4.PercentageBy = val5.FieldId;
                        }
                        goto IL_024e;
                    }
                    continue;
                }
            IL_024e:
                if (item2.PercentageOf != ScheduleFieldId.InvalidScheduleFieldId)
                {
                    ScheduleField val6 = srcDefinition.GetField(item2.PercentageBy);
                    if (!val6.IsCalculatedField && (val6.HasSchedulableField || IsValid(val6.ParameterId)))
                    {
                        ScheduleField val7 = FindField(newFields, item2);
                        ScheduleField val8 = FindField(newFields, val6);
                        if (val7 != null && val8 != null)
                        {
                            val7.PercentageBy = val8.FieldId;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 复制明细表的过滤规则
        /// </summary>
        public static void ScheduleCopyFilter(ViewSchedule des, ViewSchedule src)
        {
            ScheduleDefinition source = src.Definition;
            IList<ScheduleFilter> filters = source.GetFilters();
            des.Definition.ClearFilters();
            List<ScheduleField> newFields = des.Definition.GetFieldOrder().Select(des.Definition.GetField).ToList();
            foreach (ScheduleFilter item in filters)
            {
                ScheduleField field = source.GetField(item.FieldId);
                ScheduleField val = FindField(newFields, field);
                ScheduleFilter val2 = null;
                if (item.IsNullValue)
                {
                    val2 = new ScheduleFilter(val.FieldId, item.FilterType);
                }
                else if (item.IsDoubleValue)
                {
                    val2 = new ScheduleFilter(val.FieldId, item.FilterType, item.GetDoubleValue());
                }
                else if (item.IsElementIdValue)
                {
                    val2 = new ScheduleFilter(val.FieldId, item.FilterType, item.GetElementIdValue());
                }
                else if (item.IsIntegerValue)
                {
                    val2 = new ScheduleFilter(val.FieldId, item.FilterType, item.GetIntegerValue());
                }
                else if (item.IsStringValue)
                {
                    val2 = new ScheduleFilter(val.FieldId, item.FilterType, item.GetStringValue());
                }
                if (val2 != null)
                {
                    des.Definition.AddFilter(val2);
                }
            }
        }

        /// <summary>
        /// 复制明细表的排序规则
        /// </summary>
        public static void ScheduleCopySorting(ViewSchedule des, ViewSchedule src)
        {
            ScheduleDefinition source = src.Definition;
            IList<ScheduleSortGroupField> sortGroupFields = source.GetSortGroupFields();
            des.Definition.ClearSortGroupFields();
            List<ScheduleField> newFields = (des.Definition.GetFieldOrder()).Select(des.Definition.GetField).ToList();
            foreach (ScheduleSortGroupField item in sortGroupFields)
            {
                ScheduleField field = source.GetField(item.FieldId);
                ScheduleField val = FindField(newFields, field);
                ScheduleSortGroupField val2 = new ScheduleSortGroupField(val.FieldId, item.SortOrder);
                val2.ShowBlankLine = item.ShowBlankLine;
                if (item.ShowFooter)
                {
                    val2.ShowFooter = item.ShowFooter;
                    val2.ShowFooterCount = item.ShowFooterCount;
                    val2.ShowFooterTitle = item.ShowFooterTitle;
                }

                val2.ShowHeader = item.ShowHeader;
                des.Definition.AddSortGroupField(val2);
                des.Definition.IsItemized = source.IsItemized;
            }
        }

        /// <summary>
        /// 判断ID是否有效
        /// </summary>
        private static bool IsValid(ElementId elementId)
        {
            return elementId != null && elementId != ElementId.InvalidElementId && elementId.IntegerValue != 0;
        }
    }
}
