using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2.DataModels
{
    public enum ErrorCodes
    {
        invalid_request,
        unauthorized_client,
        access_denied,
        unsupported_response_type,
        invalid_scope,
        server_error,
        temporarily_unavailable,
    }
}
