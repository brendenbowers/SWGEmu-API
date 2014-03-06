using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2.DataModels
{
    public class TokenRequestError : Exception, ITokenRequestError
    {
        public ErrorCodes error { get; set; }
        public string error_description { get; set; }
        public Uri error_uri { get; set; }

        public TokenRequestError()
            : base()
        {
        }

        public TokenRequestError(ErrorCodes Error, string ErrorDescription, Uri ErrorUri = null, Exception InnerException = null)
         : base(ErrorDescription, InnerException)
        {
            error = Error;
            error_description = ErrorDescription;
            error_uri = ErrorUri;
        }
    }
}
