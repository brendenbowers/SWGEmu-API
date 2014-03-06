using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;

namespace OAuth2.DataModels
{
    [Route("/tokeninfo", "GET")]
    [Route("/tokeninfo/{access_token}", "GET")]
    public class TokenInfoRequest
    {
        public string access_token { get; set; }
        public bool validate_only { get; set; }

        public TokenInfoRequest()
        {
            validate_only = false;
        }
    }
}