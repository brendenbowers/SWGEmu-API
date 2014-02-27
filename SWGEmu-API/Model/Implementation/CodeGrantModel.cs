using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace OAuth2.Server.Model
{
    public class CodeGrantModel
    {
        public IAuthorizationCodeModel  AuthorizationCodeModel  { get; set; }
        public IApprovalModel           ApprovalModel           { get; set; }
        public ITokenModel              TokenModel              { get; set; }
        public Server.DataModel.AuthorizationCode Authorize(DataModels.ITokenRequest Request, DataModels.Approval Approval, DataModels.Client Client, DataModels.ResourceOwner Owner)
        {
            Server.DataModel.AuthorizationCode code = AuthorizationCodeModel.InsertAuthorizationCode(
                Extension.TokenHelper.CreateAccessToken(), Client, Owner, DateTime.Now.GetTotalSeconds(), Approval.scope, Request.redirect_uri);

            if (code == null)
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.server_error, "Error storing access code");
            }
            if (string.IsNullOrWhiteSpace(Approval.refresh_token))
            {
                Approval.refresh_token = Extension.TokenHelper.CreateAccessToken();
                if (!ApprovalModel.AddOrUpdateApproval(Approval))
                {
                    AuthorizationCodeModel.DeleteAuthorizationCode(code.authorization_code, code.client_id, code.redirect_uri);
                    throw new DataModels.TokenRequestError(DataModels.ErrorCodes.server_error, "Error updating approval");
                    
                }
            }
            
            return code;
        }

        public T Exchange<T>(DataModels.ITokenRequest Request, DataModels.Client Client)
            where T : DataModels.Token, new()
        {
            Server.DataModel.AuthorizationCode code = AuthorizationCodeModel.GetAuthorizationCode(Request.code, Client.id, Request.redirect_uri != null ? Request.redirect_uri.ToString() : null);
            if (code == null)
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.invalid_request, "Invalid token");
            }

            DataModels.Approval Approval = ApprovalModel.GetApproval(Client.id, code.resource_owner_id);

            if (!AuthorizationCodeModel.DeleteAuthorizationCode(code.authorization_code, code.client_id, code.redirect_uri))
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.access_denied, "Code already used");
            }

            return TokenModel.InsertToken<T>(
                Extension.TokenHelper.CreateAccessToken(), DataModels.TokenTypes.bearer, 3600, DateTime.Now.GetTotalSeconds(), Client.id, Approval.scope, Approval.resource_owner_id, Approval.refresh_token);
        }
    }
}