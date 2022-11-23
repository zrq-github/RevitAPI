using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZRQ.RevitTest.PrintBuiltInParameterGroup;
using Newtonsoft.Json;
using System.IO;

namespace ZRQ.RevitAPITest.Sample
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

            writeJsonFile(@"C:\Users\zrq\Desktop\xt_v9.2.0\builtInParameterGroupJsons.json", json);

            //将序列化的json字符串内容写入Json文件，并且保存
            void writeJsonFile(string path, string jsonConents)
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.WriteLine(jsonConents);
                    }
                }
            }

            return Result.Succeeded;
        }
    }
}