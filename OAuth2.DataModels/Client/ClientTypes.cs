using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuth2.DataModels
{
    public enum ClientTypes : int
    {
        web_application = 0,
        native_application = 1,
        user_agent_based_application = 2,

    }
}