using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using swgemurpcserver.rpc;
using SWGEmuAPI.Models.Account;

namespace SWGEmuAPI.Service
{
    public class AccountService : ServiceStack.ServiceInterface.Service
    {
        public Model.AccountModel AccountModel { get; set; }

        public List<AccountResponse> Get(SWGEmuAPI.Models.Account.AccontRequest Req)
        {
            if (!string.IsNullOrWhiteSpace(Req.username))
            {
                return AccountModel.GetAccount(Req.username, Req.password);
            }
            return null;
        }
    }
}