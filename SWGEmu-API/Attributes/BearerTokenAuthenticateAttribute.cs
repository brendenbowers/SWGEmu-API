using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using ServiceStack.ServiceHost;
using ServiceStack.Text;

namespace OAuth2.Server.Attributes
{
    public class BearerTokenAuthenticateAttribute : Attribute, IHasRequestFilter
    {
        public static readonly Regex MATCH_TOKEN =
            new Regex(@"^(?<token_type>[Bb][Ee][Aa][Rr][Ee][Rr]) (?<token>([A-Za-z0-9_-]|\.|\~|\+|\/)+)", RegexOptions.Compiled);

        public Model.ITokenModel TokenModel { get; set; }
        public Model.IClientModel ClientModel { get; set; }
        public Model.IResourceOwnerModel ResourceOwnerModel { get; set; }

        public bool SetUser { get; set; }

        public bool SetClient { get; set; }

        public bool SetToken { get; set; }

        public bool RequireValidUser { get; set; }

        public IHasRequestFilter Copy()
        {
            return new BearerTokenAuthenticateAttribute()
            {
                TokenModel = TokenModel,
                ClientModel = ClientModel,
                ResourceOwnerModel = ResourceOwnerModel,
                SetUser = SetUser,
                SetClient = SetClient,
                SetToken =  SetToken,
                RequireValidUser = RequireValidUser,
            };
        }

        public BearerTokenAuthenticateAttribute() : this(true, false)
        {
        }


        public BearerTokenAuthenticateAttribute(bool SetUser, bool SetClient, bool SetToken = true)
        {
            this.SetUser = SetUser;
            this.SetClient = SetClient;
            this.SetToken = SetToken;
            RequireValidUser = false;
        }

        public int Priority
        {
            get { return 0; }
        }

        public void RequestFilter(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            string auth = req.Headers.Get("Authorization");
            bool validUser = false;

            if (!string.IsNullOrWhiteSpace(auth))
            {
                Match rawToken = MATCH_TOKEN.Match(auth);

                if (rawToken.Success && rawToken.Groups["token_type"].Success && rawToken.Groups["token"].Success)
                {
                    DataModels.Token token = TokenModel.GetToken<DataModels.Token>(rawToken.Groups["token"].Value);
                    req.Items.Add("auth:rawtoken", rawToken);

                    if (SetToken)
                    {
                        req.Items.Add("auth:token",token);
                    }
                    
                    if (SetClient)
                    {
                        req.Items.Add("auth:client", ClientModel.GetClientByID(token.client_id));                        
                    }

                    if (!string.IsNullOrWhiteSpace(token.resource_owner_id) && SetUser)
                    {

                        DataModels.ResourceOwner owner = ResourceOwnerModel.GetByID(token.resource_owner_id);
                        if (owner != null)
                        {
                            req.Items.Add("auth:user", owner);
                            validUser = true;
                        }                        
                    }
                }
            }


            if (RequireValidUser && !validUser)
            {
                res.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                res.StatusDescription = "Valid bearer token required";
                res.AddHeader("WWW-Authenticate", "OAuth2 realm=\"{0}\"".Fmt(req.GetApplicationUrl()));
                res.Close();
            }
        }
    }
}