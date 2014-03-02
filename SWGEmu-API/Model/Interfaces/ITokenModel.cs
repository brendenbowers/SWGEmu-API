using System;
namespace OAuth2.Server.Model
{
    public interface ITokenModel
    {
        System.Collections.Generic.IEnumerable<T> GetTokenByClientID<T>(string ClientID)
            where T : DataModels.Token, new();
        System.Collections.Generic.IEnumerable<T> GetTokenByResourceOwnerID<T>(string ResourceOwnerID)
            where T : DataModels.Token, new();
        T GetToken<T>(string AccessToken)
            where T : DataModels.Token, new();
        bool InsertToken(OAuth2.DataModels.Token Token);
        T InsertToken<T>(string AccessToken, DataModels.TokenTypes TokenType, long ExpiresIn, long IssuedTime, string ClientID, string Scope = "", string ResourceOwnerID = "", string RefreshToken = null)
            where T : DataModels.Token, new();
        T InsertToken<T>(string AccessToken, DataModels.TokenTypes TokenType, long ExpiresIn, long IssuedTime, DataModels.Client Client, string Scope = "", DataModels.ResourceOwner ResourceOwner = null, string RefreshToken = null)
            where T : DataModels.Token, new();
        T InsertToken<T>(string AccessToken, DataModels.TokenTypes TokenType, long ExpiresIn, long IssuedTime, DataModels.Client Client, System.Collections.Generic.IEnumerable<DataModels.Scope> Scope, DataModels.ResourceOwner ResourceOwner = null, string RefreshToken = null)
            where T : DataModels.Token, new();
        bool DeleteToken(OAuth2.DataModels.Token Token);
        bool DeleteToken(string AccessToken, string ClientID, string ResourceOwnerID);

    }
}
