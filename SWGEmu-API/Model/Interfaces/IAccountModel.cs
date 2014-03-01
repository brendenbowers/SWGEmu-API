using SWGEmuAPI.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model
{
    public interface IAccountModel
    {
        List<AccountResponse> GetAccount(string Username, string password = null);
        List<AccountResponse> GetAccount(ulong AccountID, string password = null);
    }
}
