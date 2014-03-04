using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Security;
using OAuth2.Server.Extension;
using SWGEmuAPI.Model;
using SWGEmuAPI.Models.Account;

namespace OAuth2.Server.Model
{
    public class PasswordGrantModel
    {

        public IAccountModel AccountModel                           { get; set; } //injected by IOC
        public IResourceOwnerModel ResourceOwnerModel               { get; set; } //injected by IOC
        public ITokenModel TokenModel                               { get; set; } //injected by IOC
                

        public T Authorize<T>(DataModels.ITokenRequest Request, OAuth2.DataModels.Client Client = null)
            where T : DataModels.Token, new()
        {

            var accounts = AccountModel.GetAccount(Request.username, Request.password);

            if (accounts == null || accounts.Count == 0)
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.invalid_request, "Invalid username or password", Request);
            }

            DataModels.ResourceOwner owner = new DataModels.ResourceOwner()
            {
                id = accounts[0].username,
                time = DateTime.UtcNow.Millisecond,
                attributes = accounts[0].ToDictonary(),
            };

            ResourceOwnerModel.CreateOrUpdate(owner);
            T token =
                TokenModel.InsertToken<T>(
                    TokenHelper.CreateAccessToken(),
                    DataModels.TokenTypes.bearer,
                    3600,
                    DateTime.UtcNow.GetTotalSeconds(),
                    Client,
                    TokenHelper.IntersectScopes(Request.scope, Client.allowed_scope),
                    owner);

            if (token == null)
            {
                throw new OAuth2.DataModels.TokenRequestError(DataModels.ErrorCodes.server_error, "Unable to store access token");
            }

            return token;
        }
    }
}