using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public static class PlatformExtension
    {
        public static bool IsMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}