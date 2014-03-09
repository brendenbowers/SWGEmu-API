using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using OAuth2.Server.Model.DataModel;
using OAuth2.Server.Extension;
using OAuth2.Server.Model;
using ServiceStack.ServiceHost;
using OAuth2.Server.Extension;
using SWGEmuAPI.Model;
using OAuth2.DataModels;

namespace OAuth2.Server.Service
{
    [DefaultView("LoginView")]
    public class Auth : ServiceStack.ServiceInterface.Service
    {
        public IAccountModel        AccountModel            { get; set; } //injected by IOC
        public IResourceOwnerModel  ResourceOwnerModel      { get; set; } //injected by IOC
        public ITokenModel          TokenModel              { get; set; } //injected by IOC
        

        public object Get(LoginRequest LoginDetails)
        {
            DataModels.ResourceOwner owner = Session.Get<DataModels.ResourceOwner>("AuthResourceOwner");
            if (owner != null)
            {
                Uri redirectURI = null;
                Uri current = new Uri(Request.AbsoluteUri);

                if (!Uri.TryCreate(LoginDetails.redirect, UriKind.RelativeOrAbsolute, out redirectURI) || (redirectURI.IsAbsoluteUri && current.Host != redirectURI.Host))
                {
                    LoginDetails.errors = new string[] 
                    {
                        "Invalid Redirect URI",
                    };

                    return new HttpResult(LoginDetails)
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        StatusDescription = "Invalid Redirect URI",
                    };
                }
                else
                {
                    return new HttpResult(LoginDetails)
                    {
                        StatusCode = System.Net.HttpStatusCode.Redirect,
                        Headers = { { HttpHeaders.Location, redirectURI.ToString() } },
                    };
                }
            }

            return LoginDetails;
        }

        public object Post(LoginRequestWithCredentials LoginDetails)
        {
            Uri referrerURI = Request.GetReferrerURI();
            Uri current = new Uri(Request.AbsoluteUri);

            string userPassword = LoginDetails.password;
            //unset the password so we can use the LoginDetails in the resulting display if there is an error
            LoginDetails.password = null;

            Request.Items.Add("Model", LoginDetails);

            //CRSF protection
            if (!referrerURI.SchemeHostPathMatch(current))
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.invalid_request, "Invalid Request", LoginDetails);
            }


            if (string.IsNullOrWhiteSpace(LoginDetails.username) || string.IsNullOrWhiteSpace(userPassword))
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.invalid_request, "Missing Username or Password", LoginDetails);
            }

            
            OAuth2.DataModels.ResourceOwner owner = null;

            List<SWGEmuAPI.Model.Account.AccountResponse> accounts = AccountModel.GetAccount(LoginDetails.username, userPassword);


            if (accounts == null || accounts.Count == 0)
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.invalid_request, "Invalid Username or Password", LoginDetails);
            }

            try
            {
                owner = ResourceOwnerModel.CreateOrUpdateFromAccountModel(accounts.FirstOrDefault());
            }
            catch (System.Data.Common.DbException dbex)
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.server_error, "Error Storing Resource Owner Details", LoginDetails, null, dbex);
            }

            Session.Set<OAuth2.DataModels.ResourceOwner>("AuthResourceOwner", owner);

            Uri redirectURI = null;
            bool valid = Uri.TryCreate(LoginDetails.redirect, UriKind.RelativeOrAbsolute, out redirectURI);
            if (!valid || (redirectURI.IsAbsoluteUri && current.Host != redirectURI.Host))
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.invalid_request, "Invalid Redirect URI", LoginDetails);
            }

            return new HttpResult(LoginDetails)
            {
                StatusCode = System.Net.HttpStatusCode.Redirect,
                Headers = {{ HttpHeaders.Location, LoginDetails.redirect }},
            };
        }

        public object Get(LogoutRequest LogoutDetails)
        {
            if (!string.IsNullOrWhiteSpace(LogoutDetails.access_token))
            {
                var token = TokenModel.GetToken<DataModels.Token>(LogoutDetails.access_token);
                if (token != null)
                {
                    TokenModel.DeleteToken(token);
                }
            }

            var sessionid = this.GetSessionId();
            if (!string.IsNullOrWhiteSpace(sessionid) && Cache != null)
            {
                Cache.Remove(string.Format("sess:{0}:AuthResourceOwner", sessionid));
            }

            Request.Items.Remove("AuthResourceOwner");
            this.RemoveSession();
            return new HttpResult()
            {
                StatusCode = System.Net.HttpStatusCode.Redirect,
                Location = Request.GetReferrerURI().AbsolutePath,
            };
        }
    }
}