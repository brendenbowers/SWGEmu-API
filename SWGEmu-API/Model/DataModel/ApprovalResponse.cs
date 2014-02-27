using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;

namespace OAuth2.Server.Model.DataModel
{
    [Route("/approval", Verbs = "POST")]
    public class ApprovalResponse : ApprovalRequest
    {
        public string approved_scopes { get; set; }
        public string approval { get; set; }
    }
}