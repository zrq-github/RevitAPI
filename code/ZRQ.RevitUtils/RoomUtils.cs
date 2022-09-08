using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWTransCommon.HWRevit
{
    /// <summary>
    /// Room operation class
    /// </summary>
    public class RoomUtils
    {
        /// <summary>
        /// 获取房间的封闭线
        /// </summary>
        /// <param name="room">房间</param>
        /// <param name="options">分界线元素的设置</param>
        /// <remarks>
        /// 根据测试, 房间必须在分界元素和标高有交集才能创建
        /// </remarks>
        public static List<CurveLoop> GetCurveLoops(Room room, SpatialElementBoundaryOptions options = null)
        {
            if (null == options)
            {
                options = new SpatialElementBoundaryOptions();
            }

            var roomBoundarySegments = room.GetBoundarySegments(options);
            if (roomBoundarySegments.Count == 0)
            {
                return new List<CurveLoop>();
            }

            List<CurveLoop> loops = new List<CurveLoop>();
            foreach (IList<BoundarySegment> segments in roomBoundarySegments)
            {
                List<Curve> list = segments.ToList().ConvertAll(p => p.GetCurve());
                loops.Add(CurveLoop.Create(list));
            }
            return loops;
        }
    }
}
