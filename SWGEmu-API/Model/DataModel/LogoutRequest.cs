using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;
using OAuth2.DataModels;

namespace OAuth2.Server.Model.DataModel
{
    [Route("/auth/logout", Verbs = "GET")]
    [Route("/auth/logout/{access_token}", Verbs = "GET")]
    public class LogoutRequest
    {
        public string access_token { get; set; }
    }
}