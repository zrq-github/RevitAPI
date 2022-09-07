/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/7 19:44:37
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
    internal class A
    {
        public string ADescribe { get; set; } = "A";

        public List<B> Bs { get; set; } = new List<B> { new B(), new B() };
    }
}