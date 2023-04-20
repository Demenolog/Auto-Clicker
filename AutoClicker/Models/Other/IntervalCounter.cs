using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.Models.Other
{
    internal static class IntervalCounter
    {
        public static int GetTotalIntervalTime(string hours, string minutes, string seconds, string milliseconds) =>
            (int)new TimeSpan(0,
                int.Parse(hours),
                int.Parse(minutes),
                int.Parse(seconds),
                int.Parse(milliseconds)).TotalMilliseconds;
    }
}
