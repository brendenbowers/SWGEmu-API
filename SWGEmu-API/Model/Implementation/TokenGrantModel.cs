using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ServiceStack.CacheAccess;
using OAuth2.Server.Extension;

namespace OAuth2.Server.Model
{
    public class TokenGrantModel
    {        
        public  ITokenModel                                 TokenModel  { get; set; } //injected by IOC

        public T Authorize<T>(OAuth2.DataModels.Client Client = null, OAuth2.DataModels.ResourceOwner Owner = null, string Scope = "")
            where T : DataModels.Token, new()
        {
            T token = TokenModel.InsertToken<T>(
                TokenHelper.CreateAccessToken(), DataModels.TokenTypes.bearer, 3600, DateTime.UtcNow.GetTotalSeconds(), Client, Scope, Owner);
            
            if (token == null)
            {
                throw new OAuth2.DataModels.TokenRequestError(DataModels.ErrorCodes.server_error, "Unable to store access token");
            }
            return token;
        }
    }
}