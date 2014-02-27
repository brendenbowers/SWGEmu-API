using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Dapper;
using OAuth2.Server.Extension;
using OAuth2.Server.Extension;
using ServiceStack.Redis;

namespace OAuth2.Server.Model
{
    public class TokenCacheModel : OAuth2.Server.Model.ICacheTokenModel
    {
        
        private static string TOKEN_URN = "urn:Token:AccessToken:";
        private static string CLIENT_TO_TOKEN_URN = "urn:Token:ClientToToken:";
        private static string RESOURCEOWNER_TO_TOKEN_URN = "urn:Token:ROToToken:";

        public IRedisClient Cache { get; set; }


        public IEnumerable<T> GetTokenByClientID<T>(string ClientID) 
            where T : OAuth2.DataModels.Token, new()
        {
            if (string.IsNullOrWhiteSpace(ClientID))
            {
                throw new ArgumentException("ClientID is null or empty", "ClientID");
            }

            List<string> tokenIDs = new List<string>();
            foreach (string item in Cache.GetAllItemsFromSet(CLIENT_TO_TOKEN_URN + ClientID))
            {
                tokenIDs.Add(TOKEN_URN + item);
            }


            if (tokenIDs == null || tokenIDs.Count == 0)
                return new T[] { };

            return Cache.GetAll<T>(tokenIDs).Values.ToList();
        }

        public IEnumerable<T> GetTokenByResourceOwnerID<T>(string ResourceOwnerID) where T : OAuth2.DataModels.Token, new()
        {
            if (string.IsNullOrWhiteSpace(ResourceOwnerID))
            {
                throw new ArgumentException("ResourceOwnerID is required", "ResourceOwnerID");
            }

            List<string> tokenIDs = new List<string>();
            foreach (string item in Cache.GetAllItemsFromSet(RESOURCEOWNER_TO_TOKEN_URN + ResourceOwnerID))
            {
                tokenIDs.Add(TOKEN_URN + item);
            }

            if (tokenIDs == null || tokenIDs.Count == 0)
                return new T[] { };

            return Cache.GetAll<T>(tokenIDs).Values.ToList();
        }

        public T GetToken<T>(string AccessToken) 
            where T : OAuth2.DataModels.Token, new()
        {
            if (string.IsNullOrWhiteSpace(AccessToken))
            {
                throw new ArgumentException("AccessToken is required", "AccessToken");
            }

            return Cache.Get<T>(TOKEN_URN + AccessToken);
        }

        public bool InsertToken(OAuth2.DataModels.Token Token)
        {

            AddToCacheList(CLIENT_TO_TOKEN_URN, Token.client_id, Token.access_token);

            if (!string.IsNullOrWhiteSpace(Token.resource_owner_id))
            {
                AddToCacheList(RESOURCEOWNER_TO_TOKEN_URN, Token.resource_owner_id, Token.access_token);
            }

            if (!Cache.Set<OAuth2.DataModels.Token>(TOKEN_URN + Token.access_token, (OAuth2.DataModels.Token)Token, TimeSpan.FromSeconds(Token.expires_in)))
            {
                return false;
            }

            return true;
        }

        public T InsertToken<T>(string AccessToken, OAuth2.DataModels.TokenTypes TokenType, long ExpiresIn, long IssuedTime, string ClientID, string Scope = "", string ResourceOwnerID = "", string RefreshToken = null) 
            where T : OAuth2.DataModels.Token, new()
        {
            var token = new T()
            {
                access_token = AccessToken,
                client_id = ClientID,
                expires_in = ExpiresIn,
                issue_time = IssuedTime,
                refresh_token = RefreshToken,
                resource_owner_id = ResourceOwnerID,
                scope = Scope,
                token_type = TokenType,
            };

            if (InsertToken(token))
            {
                return token;
            }

            return null;
        }

        public T InsertToken<T>(string AccessToken, OAuth2.DataModels.TokenTypes TokenType, long ExpiresIn, long IssuedTime, OAuth2.DataModels.Client Client, string Scope = "", OAuth2.DataModels.ResourceOwner ResourceOwner = null, string RefreshToken = null) 
            where T : OAuth2.DataModels.Token, new()
        {
            return InsertToken<T>(AccessToken, TokenType, ExpiresIn, IssuedTime, Client.id, Scope, ResourceOwner == null ? "" : ResourceOwner.id, RefreshToken);
        }

        public T InsertToken<T>(string AccessToken, OAuth2.DataModels.TokenTypes TokenType, long ExpiresIn, long IssuedTime, OAuth2.DataModels.Client Client, IEnumerable<OAuth2.DataModels.Scope> Scope, OAuth2.DataModels.ResourceOwner ResourceOwner = null, string RefreshToken = null) 
            where T : OAuth2.DataModels.Token, new()
        {
            return InsertToken<T>(AccessToken, TokenType, ExpiresIn, IssuedTime, Client, Scope.ToScopeString(), ResourceOwner, RefreshToken);
        }


        private void AddToCacheList(string Prefix, string Key, string AccessToken)
        {
            Cache.AddItemToSet(Prefix + Key, AccessToken);
        }
    }
}