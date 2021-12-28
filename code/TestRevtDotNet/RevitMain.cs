/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2021/12/28 17:17:51
 * 文件描述:  
 * 
*************************************************************************************/
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestRevtDotNet
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true)]
    internal class RevitMain : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            throw new NotImplementedException();
        }

        public Result OnStartup(UIControlledApplication application)
        {
            throw new NotImplementedException();
        }
    }
}