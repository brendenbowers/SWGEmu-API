using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;

namespace OAuth2.DataModels
{
    [Route("/token", Verbs="POST,GET", Summary="Endpoint to obtain a token")]
    public class TokenRequest : ITokenRequest
    {

        /// <summary>
        /// the type of token being requested (client credentials, password, token, ect.)
        /// </summary>
        public AuthTypes? response_type
        {
            get;
            set;
        }

        /// <summary>
        /// Grant type for exchanging a access code for a token
        /// </summary>
        public AuthTypes? grant_type
        {
            get;
            set;
        }

        public string code { get; set; }

        public string client_id
        {
            get;
            set;
        }

        public Uri redirect_uri
        {
            get;
            set;
        }

        public string scope
        {
            get;
            set;
        }

        public string state
        {
            get;
            set;
        }

        public string username
        {
            get;
            set;
        }

        public string refresh_token { get; set; }

        public string password
        {
            get;
            set;
        }

        public string client_password
        {
            get;
            set;
        }

        /*public System.Security.SecureString password
        {
            get;
            set;
        }

        public System.Security.SecureString client_password
        {
            get;
            set;
        }*/
    }
}