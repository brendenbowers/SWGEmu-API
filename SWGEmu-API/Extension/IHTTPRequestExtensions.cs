using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;
using ServiceStack.Common;

namespace ServiceStack.ServiceHost
{
    public static class IHTTPRequestExtensions
    {
        public static string GetReferrerURL(this IHttpRequest Request)
        {
            if (Request.Headers.AllKeys.Contains("Referer"))
            {
                return Request.Headers.Get("Referer");
            }
            return null;
        }

        public static Uri GetReferrerURI(this IHttpRequest Request)
        {
            string url = Request.GetReferrerURL();
            Uri uri = null;
            Uri.TryCreate(url, UriKind.Absolute, out uri);
            return uri;
        }

        public static string ToApplicationPath(this IHttpRequest Request, string RelativePart = null)
        {
            if (string.IsNullOrWhiteSpace(RelativePart))
                return Request.GetApplicationUrl();

            return Request.GetApplicationUrl().CombineWith(RelativePart);
        }
    }
}