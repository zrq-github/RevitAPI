/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/3 18:46:27
 * 文件描述:  
 * 
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitUtils.ExternalEventUtils
{
    public interface IExternalEventBase
    {
        public void Execute(UIApplication app);

        public string GetName();
    }
}