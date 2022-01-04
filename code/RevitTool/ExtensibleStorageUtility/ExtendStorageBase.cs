﻿/*************************************************************************************
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
        /// <summary>
        /// 当前版本号
        /// </summary>
        public abstract string CurVersion { get; set; }
        public abstract UpdataState UpdataState { get; set; }
        public abstract UpdataState UpdateData(Element ele);
    }
}