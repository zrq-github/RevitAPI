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

namespace ZRQ.RevitUtils.ExternalEventUtils
{
    [Transaction(TransactionMode.Manual)]
    public class ExternalEventHandler : IExternalEventHandler
    {
        public static ExternalEvent Event;

        public static IExternalEventBase EventBase;

        public static void CreateExternalEvent()
        {
            Event = ExternalEvent.Create(new ExternalEventHandler());
        }

        public void Execute(UIApplication app)
        {
            if (EventBase == null)
                return;
            try
            {
                EventBase.Execute(app);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                EventBase = null;
            }
        }

        public string GetName()
        {
            if (EventBase == null)
                return nameof(ExternalEventHandler);

            return EventBase.GetName();
        }
    }
}