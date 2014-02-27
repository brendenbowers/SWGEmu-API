using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;
using OAuth2.DataModels;

namespace OAuth2.Server.Model.DataModel
{
    [Route("/auth/login", Verbs = "POST")]
    public class LoginRequestWithCredentials : LoginRequest, ILoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }

    }
}