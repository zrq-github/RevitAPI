/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/3 18:13:07
 * 文件描述:  
 * 
*************************************************************************************/
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.RevitUtils.ExternalEventUtility
{
    [Transaction(TransactionMode.Manual)]
    public class ExternalEventHandler : IExternalEventHandler
    {
        public static ExternalEvent Event;

        public static IExternalEventBase IEventBase;

        public static void CreateExternalEvent()
        {
            Event = ExternalEvent.Create(new ExternalEventHandler());
        }

        public void Execute(UIApplication app)
        {
            if (IEventBase == null)
                return;
            try
            {
                IEventBase.Execute(app);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IEventBase = null;
            }
        }

        public string GetName()
        {
            if (IEventBase == null)
                return nameof(ExternalEventHandler);

            return IEventBase.GetName();
        }
    }
}