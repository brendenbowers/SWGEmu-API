using System;
namespace OAuth2.Server.Model
{
    public interface IAuthorizationCodeModel
    {
        bool DeleteAuthorizationCode(string AuthorizationCode, string ClientID, string RedirectURI = null);
        OAuth2.Server.DataModel.AuthorizationCode GetAuthorizationCode(string AuthorizationCode, string ClientID, string RedirectURI = null);
        bool InsertAuthorizationCode(OAuth2.Server.DataModel.AuthorizationCode Token);
        OAuth2.Server.DataModel.AuthorizationCode InsertAuthorizationCode(string AuthorizationCode, OAuth2.DataModels.Client Client, OAuth2.DataModels.ResourceOwner ResourceOwner, long IssueTime, System.Collections.Generic.List<OAuth2.DataModels.Scope> Scope, string RedirectURI);
        OAuth2.Server.DataModel.AuthorizationCode InsertAuthorizationCode(string AuthorizationCode, OAuth2.DataModels.Client Client, OAuth2.DataModels.ResourceOwner ResourceOwner, long IssueTime, System.Collections.Generic.List<OAuth2.DataModels.Scope> Scope, Uri RedirectURI);
        OAuth2.Server.DataModel.AuthorizationCode InsertAuthorizationCode(string AuthorizationCode, OAuth2.DataModels.Client Client, OAuth2.DataModels.ResourceOwner ResourceOwner, long IssueTime, string Scope = "", string RedirectURI = null);
        OAuth2.Server.DataModel.AuthorizationCode InsertAuthorizationCode(string AuthorizationCode, OAuth2.DataModels.Client Client, OAuth2.DataModels.ResourceOwner ResourceOwner, long IssueTime, string Scope, Uri RedirectURI);
        OAuth2.Server.DataModel.AuthorizationCode InsertAuthorizationCode(string AuthorizationCode, string ClientID, string ResourceOwnerID, long IssueTime, string Scope = "", string RedirectURI = null);
        OAuth2.Server.DataModel.AuthorizationCode InsertAuthorizationCode(string AuthorizationCode, string ClientID, string ResourceOwnerID, long IssueTime, string Scope = "", Uri RedirectURI = null);
    }
}
