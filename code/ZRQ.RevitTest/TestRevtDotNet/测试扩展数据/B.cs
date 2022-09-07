/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/7 19:45:44
 * 文件描述:  
 * 
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.Test.RevtDotNet.测试扩展数据
{
    internal class B
    {
        public string BDescribe { get; set; } = "B";

        public List<C> CDescribe { get; set; } = new List<C> { new C(), new C() };
    }
}