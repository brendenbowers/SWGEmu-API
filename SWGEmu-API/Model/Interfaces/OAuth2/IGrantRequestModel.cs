using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OAuth2.Server.Model.DataModel;
using OAuth2.DataModels;

namespace OAuth2.Server.Model
{
    public interface IGrantRequestModel
    {
        ITokenResponse Authorize(ITokenRequest Request, OAuth2.DataModels.Client Client = null, OAuth2.DataModels.ResourceOwner Owner = null, string Scope = "");
    }
}