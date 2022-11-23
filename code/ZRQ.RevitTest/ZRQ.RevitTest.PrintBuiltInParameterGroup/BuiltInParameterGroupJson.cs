using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitTest.PrintBuiltInParameterGroup
{
    public class BuiltInParameterGroupJson
    {
        /// <summary>
        /// 参数分组的名字
        /// </summary>
        public string ParameterGroupName { get; set; }

        /// <summary>
        /// 对应的枚举值
        /// </summary>
        public int ParameterValue { get; set; }

        /// <summary>
        /// 枚举名字
        /// </summary>
        public string EnumName { get; set; }
    }
}
