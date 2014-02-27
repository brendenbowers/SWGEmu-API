using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuth2.Server.Model.DataModel
{
    public class TokenResponse : ITokenResponse
    {

        public ResponseActions Action
        {
            get;
            set;
        }

        public OAuth2.DataModels.TokenResponse Response
        {
            get;
            set;
        }
    }
}