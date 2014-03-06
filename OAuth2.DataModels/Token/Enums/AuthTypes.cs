using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuth2.DataModels
{
    public enum AuthTypes : int
    {
        code = 0,
        authorization_code = 1,
        token = 2,
        password = 3,
        client_credentials = 4,
        refresh_token = 5,
    }
}