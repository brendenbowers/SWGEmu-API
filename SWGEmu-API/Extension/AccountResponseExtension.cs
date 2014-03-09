using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model.Account
{
    public static class AccountResponseExtension
    {
        public static Dictionary<string, object[]> ToDictonary(this SWGEmuAPI.Model.Account.AccountResponse res)
        {
            return new Dictionary<string, object[]> 
            {
                {"account_id",new object[] {res.account_id}},
                {"active",new object[] {res.active}},
                {"characters", res.characters.Count > 0 ? res.characters.Cast<object>().ToArray() : new object[]{}},
                {"created",new object[] {res.created}},
                {"username",new object[] {res.username}},
            };
        }
    }
}
