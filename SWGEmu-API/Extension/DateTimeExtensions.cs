using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public static class DateTimeExtensions
    {
        public static long GetTotalSeconds(this DateTime Time)
        {
            return Convert.ToInt64(Time.Subtract(DateTime.MinValue).TotalSeconds);
        }
    }
}