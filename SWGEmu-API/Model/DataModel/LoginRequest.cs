using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;
using OAuth2.DataModels;

namespace OAuth2.Server.Model.DataModel
{
 
    [Route("/auth/login", Verbs="GET")]
    public class LoginRequest : ILoginRequest
    {
        public string redirect { get; set; }
        public string[] errors { get; set; }
    }
}