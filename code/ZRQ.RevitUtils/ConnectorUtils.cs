using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitUtils
{
    internal class ConnectorUtils
    {
        /// <summary>
        /// 获取主连接点
        /// </summary>
        private static Connector GetMajorConnector(FamilyInstance fit)
        {
            Connector mainConnector = null;
            ConnectorSet fitConnSets = fit.MEPModel.ConnectorManager.Connectors;
            ConnectorSetIterator iter = fitConnSets.ForwardIterator();
            while (iter.MoveNext())
            {
                Connector c = iter.Current as Connector;
                if (c.GetMEPConnectorInfo().IsPrimary)
                {
                    mainConnector = c;
                    break;
                }
            }
            return mainConnector;
        }
    }
}
