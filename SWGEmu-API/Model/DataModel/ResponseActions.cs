using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuth2.Server.Model.DataModel
{
    public enum ResponseActions : int
    {
        Error = -1,
        RedirectFragment = 0,
        RedirectParamaters = 1,
        Render = 2,
    }
}