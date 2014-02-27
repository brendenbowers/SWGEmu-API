using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Dapper;
using OAuth2.Server.Extension;
using ServiceStack.CacheAccess;
using OAuth2.Server.Extension;

namespace OAuth2.Server.Model
{
    public class CacheDBTokenModel : OAuth2.Server.Model.ITokenModel
    {
        public OAuth2.Server.Model.ICacheTokenModel CacheTokenModel { get; set; }
        public OAuth2.Server.Model.IDBTokenModel DBTokenModel { get; set; }

        public IEnumerable<T> GetTokenByClientID<T>(string ClientID) where T : OAuth2.DataModels.Token, new()
        {
            IEnumerable<T> found = null;
            if (CacheTokenModel != null)
            {
                found = CacheTokenModel.GetTokenByClientID<T>(ClientID);
            }

            if ((found == null || !found.Any()) && DBTokenModel != null)
            {
                found = DBTokenModel.GetTokenByClientID<T>(ClientID);
            }

            return found;
        }

        public IEnumerable<T> GetTokenByResourceOwnerID<T>(string ResourceOwnerID) where T : OAuth2.DataModels.Token, new()
        {
            IEnumerable<T> found = null;
            if (CacheTokenModel != null)
            {
                found = CacheTokenModel.GetTokenByResourceOwnerID<T>(ResourceOwnerID);
            }

            if ((found == null || !found.Any()) && DBTokenModel != null)
            {
                found = DBTokenModel.GetTokenByResourceOwnerID<T>(ResourceOwnerID);
            }

            return found;
        }

        public T GetToken<T>(string AccessToken) where T : OAuth2.DataModels.Token, new()
        {
            T found = null;
            if (CacheTokenModel != null)
            {
                found = CacheTokenModel.GetToken<T>(AccessToken);
            }

            if (found == null && DBTokenModel != null)
            {
                found = DBTokenModel.GetToken<T>(AccessToken);
            }

            return found;
        }

        public bool InsertToken(OAuth2.DataModels.Token Token)
        {
            if (CacheTokenModel == null && DBTokenModel == null)
                return false;
            
            if (CacheTokenModel != null && !CacheTokenModel.InsertToken(Token))
                return false;
            
            if (DBTokenModel != null && !DBTokenModel.InsertToken(Token))
                return false;

            return true;
        }

        public T InsertToken<T>(string AccessToken, OAuth2.DataModels.TokenTypes TokenType, long ExpiresIn, long IssuedTime, string ClientID, string Scope = "", string ResourceOwnerID = "", string RefreshToken = null) where T : OAuth2.DataModels.Token, new()
        {
            if (CacheTokenModel == null && DBTokenModel == null)
                return null;

            T val = null;
            if (CacheTokenModel != null)
                val = CacheTokenModel.InsertToken<T>(AccessToken, TokenType, ExpiresIn, IssuedTime, ClientID, Scope, ResourceOwnerID, RefreshToken);
            if(DBTokenModel != null)
                val = DBTokenModel.InsertToken<T>(AccessToken, TokenType, ExpiresIn, IssuedTime, ClientID, Scope, ResourceOwnerID, RefreshToken);
            return val;
        }

        public T InsertToken<T>(string AccessToken, OAuth2.DataModels.TokenTypes TokenType, long ExpiresIn, long IssuedTime, OAuth2.DataModels.Client Client, string Scope = "", OAuth2.DataModels.ResourceOwner ResourceOwner = null, string RefreshToken = null) where T : OAuth2.DataModels.Token, new()
        {
            if (CacheTokenModel == null && DBTokenModel == null)
                return null;

            T val = null;
            if (CacheTokenModel != null)
                val = CacheTokenModel.InsertToken<T>(AccessToken, TokenType, ExpiresIn, IssuedTime, Client, Scope, ResourceOwner, RefreshToken);
            if (DBTokenModel != null)
                val = DBTokenModel.InsertToken<T>(AccessToken, TokenType, ExpiresIn, IssuedTime, Client, Scope, ResourceOwner, RefreshToken);
            return val;
        }

        public T InsertToken<T>(string AccessToken, OAuth2.DataModels.TokenTypes TokenType, long ExpiresIn, long IssuedTime, OAuth2.DataModels.Client Client, IEnumerable<OAuth2.DataModels.Scope> Scope, OAuth2.DataModels.ResourceOwner ResourceOwner = null, string RefreshToken = null) where T : OAuth2.DataModels.Token, new()
        {
            if (CacheTokenModel == null && DBTokenModel == null)
                return null;

            T val = null;
            if (CacheTokenModel != null)
                val = CacheTokenModel.InsertToken<T>(AccessToken, TokenType, ExpiresIn, IssuedTime, Client, Scope, ResourceOwner, RefreshToken);
            if (DBTokenModel != null)
                val = DBTokenModel.InsertToken<T>(AccessToken, TokenType, ExpiresIn, IssuedTime, Client, Scope, ResourceOwner, RefreshToken);
            return val;           
        }
    }
}