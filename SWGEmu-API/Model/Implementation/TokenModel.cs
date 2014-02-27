using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Dapper;
using OAuth2.Server.Extension;

namespace OAuth2.Server.Model
{
    public class TokenDBModel : OAuth2.Server.Model.IDBTokenModel
    {
        public ServiceStack.OrmLite.IDbConnectionFactory DBFactory { get; set; } //injected by IOC

        private IDbConnection _DB = null;
        protected IDbConnection Db
        {
            get
            {
                return this._DB ?? (this._DB = ServiceStack.OrmLite.OrmLiteConnectionFactoryExtensions.Open(DBFactory));
            }
        }

        public IEnumerable<T> GetTokenByResourceOwnerID<T>(string ResourceOwnerID)
            where T : DataModels.Token, new()
        {
            const string sql = "SELECT `AccessToken`.`access_token`,`AccessToken`.`client_id`,`AccessToken`.`resource_owner_id`,`AccessToken`.`issue_time`,`AccessToken`.`expires_in`, COALESCE(GROUP_CONCAT(`AccessToken_Scope`.`scope_name` SEPARATOR  ' '), '') AS scope FROM `AccessToken` LEFT JOIN `AccessToken_Scope` ON `AccessToken`.`access_token` = `AccessToken_Scope`.`access_token` WHERE `AccessToken`.`resource_owner_id` = @resourceownerid GROUP BY `AccessToken`.`access_token`,`AccessToken`.`client_id`,`AccessToken`.`resource_owner_id`,`AccessToken`.`issue_time`,`AccessToken`.`expires_in`;";
            return Db.Query<T>(sql, new { resourceownerid = ResourceOwnerID });
        }

        public IEnumerable<T> GetTokenByClientID<T>(string ClientID)
            where T : DataModels.Token, new()
        {
            const string sql = "SELECT `AccessToken`.`access_token`,`AccessToken`.`client_id`,`AccessToken`.`resource_owner_id`,`AccessToken`.`issue_time`,`AccessToken`.`expires_in`, COALESCE(GROUP_CONCAT(`AccessToken_Scope`.`scope_name` SEPARATOR  ' '), '') AS scope FROM `AccessToken` LEFT JOIN `AccessToken_Scope` ON `AccessToken`.`access_token` = `AccessToken_Scope`.`access_token` WHERE `AccessToken`.`client_id` = @clientid GROUP BY `AccessToken`.`access_token`,`AccessToken`.`client_id`,`AccessToken`.`resource_owner_id`,`AccessToken`.`issue_time`,`AccessToken`.`expires_in`;";
            return Db.Query<T>(sql, new { clientid = ClientID });
        }

        public T GetToken<T>(string AccessToken)
            where T : DataModels.Token, new()
        {
            const string sql = "SELECT `AccessToken`.`access_token`,`AccessToken`.`client_id`,`AccessToken`.`resource_owner_id`,`AccessToken`.`issue_time`,`AccessToken`.`expires_in`, COALESCE(GROUP_CONCAT(`AccessToken_Scope`.`scope_name` SEPARATOR  ' '), '') AS scope FROM `AccessToken` LEFT JOIN `AccessToken_Scope` ON `AccessToken`.`access_token` = `AccessToken_Scope`.`access_token` WHERE `AccessToken`.`access_token` = @accesstoken GROUP BY `AccessToken`.`access_token`,`AccessToken`.`client_id`,`AccessToken`.`resource_owner_id`,`AccessToken`.`issue_time`,`AccessToken`.`expires_in`;";
            return Db.Query<T>(sql, new { accesstoken = AccessToken }).FirstOrDefault();
        }

        public bool InsertToken(OAuth2.DataModels.Token Token)
        {
            using (IDbTransaction trans = Db.BeginTransaction())
            {
                int res = Db.Execute("INSERT INTO AccessToken(access_token, client_id, resource_owner_id, issue_time, expires_in, scope) VALUES (@access_token, @client_id, @resource_owner_id, @issue_time, @expires_in, @scope)",
                    Token, trans);

                if (res != 1)
                {
                    trans.Rollback();
                    return false;
                }

                const string sql = "INSERT INTO AccessToken_Scope(access_token, scope_name) VALUES(@access_token, @scope_name);";

                foreach (string scope in Token.scope.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (Db.Execute(sql, new { access_token = Token.access_token, scope_name = scope }, trans) != 1)
                    {
                        trans.Rollback();
                        return false;
                    }
                }

                trans.Commit();
                return true;
            }
        }
        
        public T InsertToken<T>(string AccessToken, DataModels.TokenTypes TokenType, long ExpiresIn, long IssuedTime, string ClientID, string Scope = "", string ResourceOwnerID = "", string RefreshToken = null)
            where T : DataModels.Token, new()
        {
            T token = new T()
            {
                access_token = AccessToken,
                token_type = TokenType,
                expires_in = ExpiresIn,
                issue_time = IssuedTime,
                client_id = ClientID,
                scope = Scope,
                resource_owner_id = ResourceOwnerID,
                refresh_token = RefreshToken,
            };

            return InsertToken((T)token) ? token : null;
        }

        public T InsertToken<T>(string AccessToken, DataModels.TokenTypes TokenType, long ExpiresIn, long IssuedTime, DataModels.Client Client, string Scope = "", DataModels.ResourceOwner ResourceOwner = null, string RefreshToken = null)
            where T : DataModels.Token, new()
        {
            return InsertToken<T>(AccessToken, TokenType, ExpiresIn, IssuedTime, Client.id, Scope, ResourceOwner.id, RefreshToken);
        }

        public T InsertToken<T>(string AccessToken, DataModels.TokenTypes TokenType, long ExpiresIn, long IssuedTime, DataModels.Client Client, IEnumerable<DataModels.Scope> Scope, DataModels.ResourceOwner ResourceOwner = null, string RefreshToken = null)
            where T : DataModels.Token, new()
        {
            string scope = "";

            if(Scope != null)
            {
                foreach(DataModels.Scope scopeDetails in Scope)
                {
                    scope += scopeDetails.scope_name + " ";
                }
                scope = scope.Trim();
            }

            return InsertToken<T>(AccessToken, TokenType, ExpiresIn, IssuedTime, Client.id, scope, ResourceOwner.id, RefreshToken);
        }
    }
}