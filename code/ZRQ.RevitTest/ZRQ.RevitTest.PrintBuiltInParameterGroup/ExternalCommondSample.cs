using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;

namespace ZRQ.RevitTest.PrintBuiltInParameterGroup
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ExternalCommondSample : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            List<BuiltInParameterGroupJson> builtInParameterGroupJsons = new();
            var enums = Enum.GetValues(typeof(BuiltInParameterGroup));
            foreach (BuiltInParameterGroup strEnum in enums)
            {
                BuiltInParameterGroupJson group = new();
                string name = LabelUtils.GetLabelFor(strEnum);
                string enumName = strEnum.ToString();
                int parameterValue = (int)strEnum;

                group.ParameterGroupName = name;
                group.EnumName = enumName;
                group.ParameterValue = parameterValue;
                builtInParameterGroupJsons.Add(group);
            }

            var json = JsonConvert.SerializeObject(builtInParameterGroupJsons);

            WriteJsonFile(@"..\builtInParameterGroupJsons.json", json);

            //将序列化的json字符串内容写入Json文件，并且保存
            void WriteJsonFile(string path, string jsonConents)
            {
                using (FileStream fs = new(path, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new(fs, Encoding.UTF8))
                    {
                        sw.WriteLine(jsonConents);
                    }
                }
            }

            return Result.Succeeded;
        }
    }
}