using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using ServiceStack.Text;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceHost;
using OAuth2.Server.Model.DataModel;
using OAuth2.Server.Extension;
using OAuth2.DataModels;


namespace OAuth2.Server.Service
{
    public class Token : ServiceStack.ServiceInterface.Service
    {
        public Model.IClientModel               ClientModel                 { get; set; }
        public Model.IApprovalModel             ApprovalModel               { get; set; }
        public Model.CodeGrantModel             CodeGrantHanldler           { get; set; }
        public Model.PasswordGrantModel         PasswordGrantHandler        { get; set; }
        public Model.ClientGrantModel           ClientGrantHanlder          { get; set; }
        public Model.RefreshTokenGrantModel     RefreshTokenGrantHandler    { get; set; }
        public Model.TokenGrantModel            TokenGrantHandler           { get; set; }
        

        public OAuth2.DataModels.TokenResponse Get(TokenRequest request)
        {

            if (request.response_type == null)
            {
                Response.StatusCode = 400;
                throw new OAuth2.DataModels.TokenRequestError()
                    {
                        error = OAuth2.DataModels.ErrorCodes.invalid_request,
                        error_description = "Missing or invalid response type. Valid: token, code",
                    };
            }

            if (request.response_type.Value != AuthTypes.code && request.response_type.Value != AuthTypes.token)
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.unsupported_response_type, "Only the 'code' and 'token' response types are supported for get operations");
            }


            OAuth2.DataModels.Client client = ValidateClient(request.client_id, request.client_password, false, false);
            Uri clientUri = ValidateRedirectURI(request.redirect_uri, client.redirect_uri);

            switch (request.response_type.Value)
            {
                case AuthTypes.token:
                    HandleTokenGrant(request, client);
                    return null;
                case AuthTypes.code:
                    HandleCodeGrant(request, client);
                    return null;
            }

            Response.StatusCode = 500;
            throw new OAuth2.DataModels.TokenRequestError() 
                { 
                    error = OAuth2.DataModels.ErrorCodes.server_error, 
                    error_description = "Unknow server error" 
                };
        }

        public OAuth2.DataModels.TokenResponse Post(TokenRequest request)
        {
            if (request.grant_type == null || request.grant_type.Value == AuthTypes.code || request.grant_type.Value == AuthTypes.token)
            {
                Response.StatusCode = 400;
                throw new OAuth2.DataModels.TokenRequestError()
                {
                    error = OAuth2.DataModels.ErrorCodes.invalid_request,
                    error_description = "Missing or invalid grant type. Valid: " + string.Join(", ", EnumValuesExtension<AuthTypes>.GetValues().Where((cur) => cur != AuthTypes.token && cur != AuthTypes.code))
                };
            }

            OAuth2.DataModels.Client client = ValidateClient(request.client_id, request.client_password);

            Uri clientUri = ValidateRedirectURI(request.redirect_uri, client.redirect_uri);

            switch (request.grant_type.Value)
            {
                case AuthTypes.client_credentials:
                    return HandleClientCredentialsGrant(request, client);
                case AuthTypes.password:
                    return HandlePasswordGrant(request, client);
                case AuthTypes.authorization_code:
                    HandleCodeExchange(request, client);
                    return null;
                case AuthTypes.refresh_token:
                    HandleRefreshTokenGrant(request, client);
                    return null;
                default:
                    throw new DataModels.TokenRequestError(DataModels.ErrorCodes.invalid_request, string.Format("Grant Type {0} is not supported", request.grant_type.Value));
            }
        }

        protected DataModels.TokenResponse HandlePasswordGrant(ITokenRequest Request, DataModels.Client Client)
        {
            DataModels.TokenResponse res = PasswordGrantHandler.Authorize<DataModels.TokenResponse>(Request, Client);
            res.state = Request.state;

            Response.StatusCode = (int)System.Net.HttpStatusCode.Created;
            return res;
        }

        protected DataModels.TokenResponse HandleClientCredentialsGrant(ITokenRequest Request, DataModels.Client Client)
        {
            DataModels.TokenResponse res = ClientGrantHanlder.Authorize<DataModels.TokenResponse>(Request, Client);
            res.state = Request.state;

            Response.StatusCode = (int)System.Net.HttpStatusCode.Created;
            return res;
        }

        protected void HandleTokenGrant(ITokenRequest TokenRequest, DataModels.Client Client)
        {
            DataModels.ResourceOwner owner = Authenticate();
            if (owner == null)
                return;

            DataModels.Approval approval = Approve(Client, owner, TokenRequest.scope);
            if (approval == null)
                return;

            DataModels.TokenResponse token = TokenGrantHandler.Authorize<DataModels.TokenResponse>(Client, owner, approval.scope);
            token.state = TokenRequest.state.UrlEncode();

            string toRedirect = GetRedirectURI(TokenRequest, Client);

            if (toRedirect == null)
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.invalid_request, "No Registerd Redirect URI");
            }

            UriBuilder bldr = new UriBuilder(toRedirect);
            if (Client.type == DataModels.ClientTypes.web_application)
            {
                bldr.Query += token.ToURIString();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(bldr.Fragment))
                {
                    bldr.Fragment = bldr.Fragment.SafeSubstring(1, bldr.Fragment.Length - 1) + '?' + token.ToURIString();
                }
                else
                {
                    bldr.Fragment = token.ToURIString();
                }
                
            }

            Response.StatusCode = (int)System.Net.HttpStatusCode.Redirect;
            Response.AddHeader("Location", bldr.ToString());
            return;
        }

        protected void HandleCodeGrant(ITokenRequest TokenRequest, DataModels.Client Client)
        {
            if (Client.type == DataModels.ClientTypes.user_agent_based_application || string.IsNullOrWhiteSpace(Client.secret))
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.unauthorized_client, "Only secure clients are supported for code grants");
            }
            
            DataModels.ResourceOwner owner = Authenticate();
            if (owner == null)
                return;

            DataModels.Approval approval = Approve(Client, owner, TokenRequest.scope);
            if (approval == null)
                return;
            
            DataModel.AuthorizationCode code = CodeGrantHanldler.Authorize(TokenRequest, approval, Client, owner);
            if (code == null)
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.server_error, "Unknown server error");
            }

            string redirect = GetRedirectURI(TokenRequest, Client);

            UriBuilder bldr = new UriBuilder(redirect);
            string queryParms = "code=" + code.authorization_code;

            if (!string.IsNullOrWhiteSpace(code.scope))
            {
                queryParms += "&scope=" + code.scope.UrlEncode();
            }

            if (TokenRequest.state != null)
            {
                queryParms += "&state=" + TokenRequest.state.UrlEncode();
            }

            bldr.Query = queryParms;


            Response.StatusCode = (int)System.Net.HttpStatusCode.Redirect;
            Response.AddHeader("Location", bldr.ToString());
            return;
        }

        protected void HandleCodeExchange(ITokenRequest TokenRequest, DataModels.Client Client)
        {
            DataModels.TokenResponse res = CodeGrantHanldler.Exchange<DataModels.TokenResponse>(TokenRequest, Client);
            if (res == null)
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.server_error, "Unknown error exchanging code");
            }

            res.state = TokenRequest.state;

            string uri = GetRedirectURI(TokenRequest, Client);
            if(string.IsNullOrWhiteSpace(uri))
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.invalid_request, "No Redirect URI provided and no Registered Redirect URI available");
            }

            UriBuilder bldr = new UriBuilder(uri);
            bldr.Query = res.ToURIString();

            Response.StatusCode = (int)System.Net.HttpStatusCode.Redirect;
            Response.AddHeader("Location", bldr.ToString());
            return;
        }

        protected void HandleRefreshTokenGrant(ITokenRequest TokenRequest, DataModels.Client Client)
        {
            if (Client.type == DataModels.ClientTypes.user_agent_based_application || string.IsNullOrWhiteSpace(Client.secret))
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.unauthorized_client, "Only secure clients are supported for refreshing tokens");
            }

            string redirectURI = GetRedirectURI(TokenRequest, Client);
            if (string.IsNullOrWhiteSpace(redirectURI))
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.invalid_request, "No Redirect URI provided and no Registered Redirect URI available");
            }
            
            DataModels.TokenResponse res = RefreshTokenGrantHandler.Exchange<DataModels.TokenResponse>(TokenRequest.refresh_token, Client.id);
            if (res == null)
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.server_error, "Unknown Error refreshing token");
            }

            res.state = TokenRequest.state;

            UriBuilder bldr = new UriBuilder(redirectURI);
            bldr.Query = res.ToURIString();

            Response.StatusCode = (int)System.Net.HttpStatusCode.Redirect;
            Response.AddHeader("Location", bldr.ToString());
        }

        protected DataModels.Client ValidateClient(string ClientID, string ClientPassword, bool CheckPasswordIfNotWebClient = true, bool RequirePasswordForWebClients = true)
        {
            if (string.IsNullOrEmpty(ClientID))
            {
                Response.StatusCode = 400;
                throw new OAuth2.DataModels.TokenRequestError()
                {
                    error = OAuth2.DataModels.ErrorCodes.invalid_request,
                    error_description = "Missing client id"
                };
            }

            OAuth2.DataModels.Client client = ClientModel.GetClientByID(ClientID);
            if (client == null)
            {
                Response.StatusCode = 400;
                throw new OAuth2.DataModels.TokenRequestError()
                {
                    error = OAuth2.DataModels.ErrorCodes.unauthorized_client,
                    error_description = "invalid client id or client password specified"
                };
            }


            if ((((client.type == DataModels.ClientTypes.web_application && RequirePasswordForWebClients) || (CheckPasswordIfNotWebClient &&  !string.IsNullOrWhiteSpace(client.secret))) && ClientPassword != client.secret))
            {
                Response.StatusCode = 400;
                throw new OAuth2.DataModels.TokenRequestError()
                {
                    error = OAuth2.DataModels.ErrorCodes.unauthorized_client,
                    error_description = "invalid client id or client password specified"
                };
            }

            return client;
        }

        protected Uri ValidateRedirectURI(Uri RequestRedirectURI, string ClientRegisteredRedirectURI)
        {
            Uri clientUri = null;
            Uri.TryCreate(ClientRegisteredRedirectURI, UriKind.Absolute, out clientUri);
            if (RequestRedirectURI != null && RequestRedirectURI.IsAbsoluteUri && clientUri != null && clientUri.SchemeHostPathMatch(RequestRedirectURI))
            {
                Response.StatusCode = 400;
                throw new OAuth2.DataModels.TokenRequestError()
                {
                    error = OAuth2.DataModels.ErrorCodes.invalid_request,
                    error_description = "Invalid redirect uri"
                };
            }
            
            return RequestRedirectURI ?? clientUri;
        }

        protected DataModels.Approval Approve(DataModels.Client Client, DataModels.ResourceOwner Owner, string Scope)
        {
            DataModels.Approval approval = ApprovalModel.GetApproval(Client, Owner);

            if (approval == null || TokenHelper.MissingScopesArray(Scope, approval.scope).Length != 0)
            {
                string apprUrl = Request.GetApplicationUrl();

                if (!apprUrl.EndsWith("/"))
                {
                    apprUrl += '/';
                }

                UriBuilder bldr = new UriBuilder(apprUrl);
                bldr.Path += "approval";

                string query = "client_id=" + Client.id + "&redirect=" + Request.AbsoluteUri.UrlEncode();
                if (Scope != null)
                {
                    query += "&scope=" + Scope.UrlEncode();
                }
                bldr.Query = query;

                Response.AddHeader("Location", bldr.ToString());
                Response.StatusCode = (int)System.Net.HttpStatusCode.Redirect;
                return null;
            }

            return approval;
        }

        protected DataModels.ResourceOwner Authenticate()
        {
            DataModels.ResourceOwner owner = Session.Get<DataModels.ResourceOwner>("AuthResourceOwner");
            if (owner == null)
            {
                string apprUrl = Request.GetApplicationUrl();

                if (!apprUrl.EndsWith("/"))
                {
                    apprUrl += '/';
                }

                UriBuilder bldr = new UriBuilder(apprUrl);
                
                bldr.Path += "auth/login";
                bldr.Query = "redirect=" + Request.AbsoluteUri.UrlEncode();

                Response.AddHeader("Location", bldr.ToString());
                Response.StatusCode = (int)System.Net.HttpStatusCode.Redirect;
                return null;
            }
            return owner;
        }

        protected string GetRedirectURI(ITokenRequest request, DataModels.Client Client)
        {
            Uri clientURI = null;
            Uri.TryCreate(Client.redirect_uri, UriKind.RelativeOrAbsolute, out clientURI);

            if (request.redirect_uri != null && request.redirect_uri.IsAbsoluteUri)
                return request.redirect_uri.ToString();

            if (clientURI != null && clientURI.IsAbsoluteUri)
                return clientURI.ToString();
            return null;            
        }
    }
}