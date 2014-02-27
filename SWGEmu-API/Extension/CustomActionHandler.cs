using ServiceStack.ServiceHost;
using ServiceStack.WebHost.Endpoints.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;
using ServiceStack.ServiceInterface;
using ServiceStack.Common.Utils;
using ServiceStack.Common.Web;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.WebHost.Endpoints.Extensions;

namespace ServiceStack.WebHost.Endpoints
{
    public class CustomActionHandler : IServiceStackHttpHandler, IHttpHandler
    {
        public Action<IHttpRequest, IHttpResponse> Action { get; set; }

        public CustomActionHandler(Action<IHttpRequest, IHttpResponse> action)
        {
            if (action == null)
                throw new Exception("Action was not supplied to ActionHandler");

            Action = action;
        }

        public void ProcessRequest(IHttpRequest httpReq, IHttpResponse httpRes, string operationName)
        {
            Action(httpReq, httpRes);
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(context.Request.ToRequest(GetType().Name),
                context.Response.ToResponse(),
                GetType().Name);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}