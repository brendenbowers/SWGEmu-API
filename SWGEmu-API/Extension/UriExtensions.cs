using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuth2.Server.Extension
{
    public static class UriExtensions
    {
        public static bool SchemeHostPathMatch(this Uri Source, Uri Target)
        {
            return Source.GetLeftPart(UriPartial.Path) == Target.GetLeftPart(UriPartial.Path);
        }
    }
}