using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
//using ServiceStack.OrmLite;
using OAuth2.Server.Extension;
using Dapper;

namespace OAuth2.Server.Model
{
    public class ClientGrantModel
    {
        public ITokenModel TokenModel { get; set; } //injected by IOC
        public ServiceStack.OrmLite.IDbConnectionFactory DBFactory { get; set; } //injected by IOC
        private IDbConnection _DB = null;

        protected IDbConnection Db
        {
            get
            {
                return this._DB ?? (this._DB = ServiceStack.OrmLite.OrmLiteConnectionFactoryExtensions.Open(DBFactory));
            }
        }

        public T Authorize<T>(DataModels.ITokenRequest Request, OAuth2.DataModels.Client Client = null)
            where T : DataModels.Token, new()
        {

            T token = TokenModel.InsertToken<T>(
                TokenHelper.CreateAccessToken(),
                DataModels.TokenTypes.bearer,
                3600,
                DateTime.UtcNow.GetTotalSeconds(),
                Client,
                Client.allowed_scope);
            
            int res = Db.Execute(
                "INSERT INTO AccessToken (access_token, client_id, expires_in, scope) VALUES (@access_token, @client_id, @expires_in, @scope);",
                token
            );

            if (token == null)
            {
                throw new OAuth2.DataModels.TokenRequestError()
                {                    
                    error= DataModels.ErrorCodes.server_error,
                    error_description = "Unable to store token",
                };
            }

            return token;
        }
    }
}