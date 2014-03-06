using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;



namespace OAuth2.DataModels
{
    [Route("/api/approval", "GET,DELETE")]
    [Route("/api/approval/{client_id}", "GET,DELETE")]
    public class ApprovalRequest
    {
        public string client_id { get; set; }
        public string resource_owner_id { get; set; }
    }
}
