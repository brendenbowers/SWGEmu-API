using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public static class StringExtension
    {
        public static int ToInt(this string Src)
        {
            return int.Parse(Src);
        }

        public static uint ToUInt(this string Src)
        {
            return uint.Parse(Src);
        }

        public static long ToLong(this string Src)
        {
            return long.Parse(Src);
        }

        public static ulong ToULong(this string Src)
        {
            return ulong.Parse(Src);
        }

        public static float ToFloat(this string Src)
        {
            return float.Parse(Src);
        }

        public static double ToDouble(this string Src)
        {
            return double.Parse(Src);
        }
    }
}