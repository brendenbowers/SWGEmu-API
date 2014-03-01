using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using swgemurpcserver.rpc;
using SWGEmuAPI.Models.Account;

namespace SWGEmuAPI.Service
{
    [OAuth2.Server.Attributes.BearerTokenAuthenticate]
    public class AccountService : ServiceStack.ServiceInterface.Service
    {
        public Model.IAccountModel AccountModel { get; set; }

        public List<AccountResponse> Get(SWGEmuAPI.Models.Account.AccontRequest Req)
        {
            OAuth2.DataModels.ResourceOwner ro = this.Request.Items.GetValue<OAuth2.DataModels.ResourceOwner>("auth:user");
            ulong roAccountID = ro.GetSingle<string>("account_id").ToULong(); 
            ///TODO: implement admin checking to allow pulling of any account.
            if(Req.account_id == 0 && string.IsNullOrWhiteSpace(Req.username)) {
                Req.account_id = roAccountID;
            }
            else
            {
                if (Req.account_id != 0 && Req.account_id != roAccountID)
                {
                    throw new ArgumentException("Provided 'account_id' does not match that of the authorized user", "account_id");
                }

                if (!string.IsNullOrWhiteSpace(Req.username) && Req.username.ToLowerInvariant() != ro.GetSingle<string>("username").ToLowerInvariant())
                {
                    throw new ArgumentException("Provided 'username' does not match that of the authorized user", "username");
                }
            }

            ///TODO: implement scope check

            if (Req.account_id != 0)
            {
                return AccountModel.GetAccount(Req.account_id, Req.password);
            }
            if (!string.IsNullOrWhiteSpace(Req.username))
            {
                return AccountModel.GetAccount(Req.username, Req.password);
            }
            return null;
        }
    }
}