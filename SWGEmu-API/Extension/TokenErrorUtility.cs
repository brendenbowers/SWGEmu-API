using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Common.Web;
using OAuth2.DataModels;

namespace OAuth2.Server.Extension
{
    public static class TokenErrorUtility
    {
        public static HttpError CreateError(TokenRequestError Error, object DTO = null)
        {
            System.Net.HttpStatusCode Code = System.Net.HttpStatusCode.InternalServerError;

            switch (Error.error)
            {
                case ErrorCodes.invalid_request :
                case ErrorCodes.invalid_scope :
                case ErrorCodes.unsupported_response_type :
                    Code = System.Net.HttpStatusCode.BadRequest;
                    break;
                case ErrorCodes.unauthorized_client :
                case ErrorCodes.access_denied :
                    Code = System.Net.HttpStatusCode.Forbidden;
                    break;
                default:
                    Code = System.Net.HttpStatusCode.InternalServerError;
                    break;
            }

            return new HttpError(Code, Error) { Response = DTO };
        }

        public static HttpError CreateError(ErrorCodes ErrorCode, string ErrorDescription, object DTO = null, Uri ErrorURI = null, Exception InnerException = null)
        {
            return CreateError(new TokenRequestError(ErrorCode, ErrorDescription, ErrorURI, InnerException), DTO);
        }
    }
}