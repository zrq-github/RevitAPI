using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZRQ.RevitAPI.Test
{
    [Transaction(TransactionMode.Manual)]
    public class Test_明细表添加规则 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;
            View view = commandData.View;

            AssemblyInstance assemblyInstance = doc.GetElement(new ElementId(364478)) as AssemblyInstance;

            // 找到明细表
            var colViewSchedule = new FilteredElementCollector(doc);
            var viewSchedules = colViewSchedule.OfCategory(BuiltInCategory.OST_Schedules);
            ViewSchedule defaultSchedule = null;
            foreach (var schedule in viewSchedules)
            {
                if (schedule.Name.Equals("预制管段表"))
                {
                    defaultSchedule = schedule as ViewSchedule;
                }
            }
            if (defaultSchedule == null)
                return Result.Failed;



            Transaction colorIt = new Transaction(doc, "复制明细表");
            colorIt.Start();

            ElementId newScheduleId = defaultSchedule.Duplicate(ViewDuplicateOption.Duplicate);
            ViewSchedule newViewSchedule = doc.GetElement(newScheduleId) as ViewSchedule;
            newViewSchedule.Name = assemblyInstance.Name;

            //// 修改明细表的过滤规则
            ModifyFilter(doc, newViewSchedule);

            colorIt.Commit();





            //// 复制明细表字段
            //ViewScheduleTool.ScheduleCopyFields(newViewSchedule, defaultSchedule);
            //ViewScheduleTool.ScheduleCopyFilter(newViewSchedule, defaultSchedule);
            //ViewScheduleTool.ScheduleCopySorting(newViewSchedule, defaultSchedule);
            //ViewScheduleTool.ScheduleCopyAppearance(newViewSchedule, defaultSchedule);




            return Result.Succeeded;
        }

        private void ModifyFilter(Document doc, ViewSchedule newViewSchedule)
        {
            ScheduleDefinition srcDefinition = newViewSchedule.Definition;
            srcDefinition.ClearFilters();// 清楚所有比较

            // 找到共享参数的ID
            ElementId sharedParameterElementId = null;

            BindingMap bindingMap = doc.ParameterBindings;
            var it = bindingMap.ForwardIterator();
            it.Reset();
            while (it.MoveNext())
            {
                var definition = (InternalDefinition)it.Key;

                var sharedParameterElement = doc.GetElement(
                  definition.Id) as SharedParameterElement;

                if (sharedParameterElement != null)
                {
                    if (sharedParameterElement.GuidValue == new Guid("9830908a-0e2f-4e77-9004-e430b4d56402"))
                    {
                        sharedParameterElementId = sharedParameterElement.Id;
                    }
                }
            }

            // 找到需要添加规则的字段 - 预制管段编号
            ScheduleField scheduleField = null;
            List<ScheduleField> srcFields = srcDefinition.GetFieldOrder().Select(srcDefinition.GetField).ToList();
            foreach (ScheduleField srcField in srcFields)
            {
                if (srcField.ParameterId == sharedParameterElementId)
                {
                    scheduleField = srcField;
                }
            }

            ScheduleFilter scheduleFilter = new ScheduleFilter(scheduleField.FieldId, ScheduleFilterType.Equal, "AAAA");
            srcDefinition.AddFilter(scheduleFilter);
        }
    }

    public static class Test_AA
    {
        public static Parameter GetParameter(this Element element, string name)
        {
            if (element == null || !element.IsValidObject)
            {
                return null;
            }
#if REVIT2014
            return element.get_Parameter(name);
#else
            IList<Parameter> parameters = element.GetParameters(name);
            if (parameters != null && parameters.Count > 0)
            {
                return parameters[0];
            }
            return null;
#endif
        }
    }
}
