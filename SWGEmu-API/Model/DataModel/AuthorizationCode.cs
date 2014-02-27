using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2.Server.DataModel
{
    public class AuthorizationCode
    {
        public string authorization_code { get; set; }
        public string client_id { get; set; }
        public string resource_owner_id { get; set; }
        public string redirect_uri { get; set; }
        public long issue_time { get; set; }
        public string scope { get; set; }
    }
}
