using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZRQ.RevitUtils
{
    internal static class LevelUntils
    {
        public static List<Level> GetAllLevels(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Level));
            return collector.ToElements().ToList().ConvertAll(p => p as Level);
        }
    }
}
