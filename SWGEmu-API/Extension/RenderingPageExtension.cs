using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;
using ServiceStack.Razor;

namespace ServiceStack.Html
{
    public static class RenderingPageExtensions
    {
        public static string ToApplicationPath(this RenderingPage Page, string RelativePart = null)
        {
            return Page.Request.ToApplicationPath(RelativePart);
        }
    }
}