using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace OAuth2.Server.Model
{
    public class RefreshTokenGrantModel
    {
        public ITokenModel TokenModel { get; set; }
        public IApprovalModel ApprovalModel { get; set; }

        public ServiceStack.OrmLite.IDbConnectionFactory DBFactory { get; set; } //injected by IOC

        private IDbConnection _DB = null;
        protected IDbConnection Db
        {
            get
            {
                return this._DB ?? (this._DB = ServiceStack.OrmLite.OrmLiteConnectionFactoryExtensions.Open(DBFactory));
            }
        }


        public T Exchange<T>(string RefreshToken, string ClientID)
            where T : DataModels.Token, new()
        {
            DataModels.Approval approval = ApprovalModel.GetApprovalByRefreshToken(ClientID, RefreshToken);

            if (RefreshToken != approval.refresh_token)
            {
                throw new DataModels.TokenRequestError(DataModels.ErrorCodes.access_denied, "Invalid refresh token");
            }

            return TokenModel.InsertToken<T>(
                Extension.TokenHelper.CreateAccessToken(), DataModels.TokenTypes.bearer, 3600, DateTime.Now.GetTotalSeconds(), ClientID, approval.scope, approval.resource_owner_id, approval.refresh_token);

        }
    }
}