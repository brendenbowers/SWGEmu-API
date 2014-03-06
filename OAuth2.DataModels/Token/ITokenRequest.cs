using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace OAuth2.DataModels
{
    public interface ITokenRequest
    {
        AuthTypes? response_type { get; set; }
        AuthTypes? grant_type { get; set; }
        string code { get; set; }
        string client_id { get; set; }
        Uri redirect_uri { get; set; }
        string scope { get; set; }
        string state { get; set; }
        string username { get; set; }
        string refresh_token { get; set; }
        string password { get; set; }
        string client_password { get; set; }
    }
}
