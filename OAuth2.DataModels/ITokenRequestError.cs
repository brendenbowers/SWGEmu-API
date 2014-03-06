using System;
namespace OAuth2.DataModels
{
    public interface ITokenRequestError
    {
        ErrorCodes error { get; set; }
        string error_description { get; set; }
        Uri error_uri { get; set; }
    }
}
