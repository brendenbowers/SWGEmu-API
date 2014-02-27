using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;

namespace OAuth2.Server.Model.DataModel
{
    [Route("/approval", Verbs = "GET")]
    public class ApprovalRequest
    {
        public string redirect { get; set; }
        public string client_id { get; set; }
        public string scope { get; set; }
    }
}