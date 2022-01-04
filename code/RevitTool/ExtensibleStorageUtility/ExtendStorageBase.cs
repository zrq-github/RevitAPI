/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/2 20:28:48
 * 文件描述:  
 * 
*************************************************************************************/
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.RevitUtils.ExtensibleStorageUtility
{
    /// <summary>
    /// IUpdateStorage的默认实现
    /// </summary>
    public abstract class ExtendStorageBase : IExtendStorageBase
    {
        public abstract string CurVersion { get; set; }
        public virtual object UpdataNewClass { get; set; } = null;
        public abstract UpdataResult UpdataState { get; set; }
        public abstract UpdataResult UpdateData();
    }
}