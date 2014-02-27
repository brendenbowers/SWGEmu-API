using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OAuth2.Server.Model.DataModel;
using OAuth2.DataModels;

namespace OAuth2.Server.Model.DataModel
{
    public interface ITokenResponse
    {
        ResponseActions Action { get; set; }
        OAuth2.DataModels.TokenResponse Response { get; set; }
    }
}