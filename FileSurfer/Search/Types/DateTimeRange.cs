using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSurfer.Search.Types
{
    public class DateTimeRange
    {
        public DateTime MinimumTime { get; set; }

        public DateTime MaximumTime { get; set; }
    }

    public static class DataRangeDateTimeExtension
    {
        public static bool InRange(this DateTime time, in DateTimeRange range)
        {
            return time > range.MinimumTime && time < range.MaximumTime;
        }
    }
}
