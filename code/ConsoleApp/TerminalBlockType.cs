/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/7 14:31:31
 * 文件描述:  
 * 
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.ConsoleApp
{
    public enum TerminalBlockType
    {
        /// <summary>
        /// 无效类型
        /// </summary>
        Invalid = -1,
        /// <summary>
        /// 三线制
        /// </summary>
        ThreeWireSystem,
        /// <summary>
        /// 四线制
        /// </summary>
        FourWireSystem,
        /// <summary>
        /// 五线制
        /// </summary>
        FiveWireSystem,
    }
}