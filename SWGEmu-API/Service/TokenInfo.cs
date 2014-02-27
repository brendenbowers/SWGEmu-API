using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using ServiceStack.Text;
using ServiceStack.ServiceInterface;
using OAuth2.Server.Model.DataModel;
using OAuth2.Server.Extension;
using OAuth2.Server.Model;
using OAuth2.DataModels;

namespace OAuth2.Server.Service
{
    public class TokenInfo : ServiceStack.ServiceInterface.Service
    {
        public ITokenModel          TokenModel          { get; set; } //injected by IOC
        public IResourceOwnerModel  ResourceOwnerModel  { get; set; } //injected by IOC


        public DataModels.TokenInfo Get(TokenInfoRequest TokenInfoRequest)
        {
            if (string.IsNullOrWhiteSpace(TokenInfoRequest.access_token))
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.invalid_request, "Missing access token");
            }

            DataModels.TokenInfo info = TokenModel.GetToken<DataModels.TokenInfo>(TokenInfoRequest.access_token);

            if (info == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                return null;
            }


            Response.StatusCode = (int)System.Net.HttpStatusCode.Found;
            if (TokenInfoRequest.validate_only)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(info.resource_owner_id))
            {
                info.owner = ResourceOwnerModel.GetByID(info.resource_owner_id);
            }

            return info;

            Response.Write(info.ToJson());
            Response.Close();
            return null;
            return info;
        }

    }
}