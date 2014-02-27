using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Dapper;

namespace OAuth2.Server.Model
{
    public class AuthorizationCodeModel : OAuth2.Server.Model.IAuthorizationCodeModel
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

        public Server.DataModel.AuthorizationCode GetAuthorizationCode(string AuthorizationCode, string ClientID, string RedirectURI = null)
        {
            const string sql = "SELECT `AuthorizationCode`.`authorization_code`, `AuthorizationCode`.`client_id`, `AuthorizationCode`.`resource_owner_id`, `AuthorizationCode`.`redirect_uri`, `AuthorizationCode`.`issue_time`, GROUP_CONCAT(`AuthorizationCode_Scope`.`scope_name` SEPARATOR  ' ')" +
                               "FROM `AuthorizationCode`, `AuthorizationCode_Scope`" +
                               "WHERE `AuthorizationCode`.`authorization_code` = `AuthorizationCode_Scope`.`authorization_code` AND `AuthorizationCode`.`authorization_code` = @authorizationcode AND `AuthorizationCode`.`client_id` = @clientid AND (COALESCE(`AuthorizationCode`.`redirect_uri`,'NULL') = COALESCE(@redirecturi, 'NULL'))" +
                               "GROUP BY `AuthorizationCode`.`authorization_code`, `AuthorizationCode`.`client_id`, `AuthorizationCode`.`resource_owner_id`, `AuthorizationCode`.`redirect_uri`, `AuthorizationCode`.`issue_time`;";
            return Db.Query<Server.DataModel.AuthorizationCode>(sql, new { authorizationcode = AuthorizationCode, clientid = ClientID, redirecturi = RedirectURI }).FirstOrDefault();
        }

        public bool DeleteAuthorizationCode(string AuthorizationCode, string ClientID, string RedirectURI = null)
        {
            const string sql = "DELETE FROM AuthorizationCode WHERE authorization_code = @authorizationcode AND client_id = @clientid AND (COALESCE(redirect_uri,'NULL') = COALESCE(@redirecturi, 'NULL'))";
            return Db.Execute(sql, new { authorizationcode = AuthorizationCode, clientid = ClientID, redirecturi = RedirectURI }) == 1;
        }

        public bool InsertAuthorizationCode(Server.DataModel.AuthorizationCode Token)
        {
            const string tokenSQL = "INSERT INTO AuthorizationCode(authorization_code, client_id, resource_owner_id, redirect_uri, issue_time, scope) VALUES(@authorization_code, @client_id, @resource_owner_id, @redirect_uri, @issue_time, @scope);";
            const string scopeSQL = "INSERT INTO AuthorizationCode_Scope(authorization_code, scope_name) VALUES(@authorization_code, @scope_name);";

            using (IDbTransaction trans = Db.BeginTransaction())
            {
                int res = Db.Execute(tokenSQL, Token, trans);
                if (res != 1)
                {
                    trans.Rollback();
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(Token.scope))
                {
                    foreach (string scope in Token.scope.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (Db.Execute(scopeSQL, new { authorization_code = Token.authorization_code, scope_name = scope }, trans) != 1)
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                }

                trans.Commit();
                return true;
            }
        }

        public Server.DataModel.AuthorizationCode InsertAuthorizationCode(string AuthorizationCode, string ClientID, string ResourceOwnerID, long IssueTime, string Scope = "", string RedirectURI = null)
        {
            Server.DataModel.AuthorizationCode code = new Server.DataModel.AuthorizationCode()
            {
                authorization_code = AuthorizationCode,
                client_id = ClientID,
                resource_owner_id = ResourceOwnerID,
                issue_time = IssueTime, 
                redirect_uri = RedirectURI,
                scope = Scope,
            };

            if (InsertAuthorizationCode(code))
                return code;
            return null;
        }

        public Server.DataModel.AuthorizationCode InsertAuthorizationCode(string AuthorizationCode, string ClientID, string ResourceOwnerID, long IssueTime, string Scope = "", Uri RedirectURI = null)
        {
            return InsertAuthorizationCode(AuthorizationCode, ClientID, ResourceOwnerID, IssueTime, Scope, (RedirectURI != null) ? RedirectURI.ToString() : null); 
        }

        public Server.DataModel.AuthorizationCode InsertAuthorizationCode(string AuthorizationCode, DataModels.Client Client, DataModels.ResourceOwner ResourceOwner, long IssueTime, string Scope = "", string RedirectURI = null)
        {
            return InsertAuthorizationCode(AuthorizationCode, Client.id, ResourceOwner.id, IssueTime, Scope, RedirectURI); 
        }

        public Server.DataModel.AuthorizationCode InsertAuthorizationCode(string AuthorizationCode, DataModels.Client Client, DataModels.ResourceOwner ResourceOwner, long IssueTime, string Scope, Uri RedirectURI)
        {
            return InsertAuthorizationCode(AuthorizationCode, Client.id, ResourceOwner.id, IssueTime, Scope, (RedirectURI != null) ? RedirectURI.ToString() : null); 
        }

        public Server.DataModel.AuthorizationCode InsertAuthorizationCode(string AuthorizationCode, DataModels.Client Client, DataModels.ResourceOwner ResourceOwner, long IssueTime, List<DataModels.Scope> Scope, string RedirectURI)
        {
            string scope = "";

            if (Scope != null)
            {
                foreach (DataModels.Scope scopeDesc in Scope)
                {
                    scope += scopeDesc.scope_name + " ";
                }
                scope = scope.Trim();
            }

            return InsertAuthorizationCode(AuthorizationCode, Client, ResourceOwner, IssueTime, scope, RedirectURI);
        }

        public Server.DataModel.AuthorizationCode InsertAuthorizationCode(string AuthorizationCode, DataModels.Client Client, DataModels.ResourceOwner ResourceOwner, long IssueTime, List<DataModels.Scope> Scope, Uri RedirectURI)
        {
            return InsertAuthorizationCode(AuthorizationCode, Client, ResourceOwner, IssueTime, Scope, (RedirectURI != null) ? RedirectURI.ToString() : null);
        }
    }
}