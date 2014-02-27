using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using swgemurpcserver.rpc;
using SWGEmuAPI.Models.Account;
using DeltaVSoft.RCFProto;

namespace SWGEmuAPI.Model
{
    public class AccountModel : IAccountModel
    {
        public SWGEmuAccountService.Stub RPCServiceStub { get; set; }

        public List<AccountResponse> GetAccount(string Username, string password = null)
        {
            GetAccountRequest req;
            if (!string.IsNullOrWhiteSpace(password))
            {
                req = GetAccountRequest.CreateBuilder()
                    .SetUserName(Username)
                    .SetPassword(password)
                    .SetSearchType(GetAccountRequest.Types.SearchType.ACCOUNTNAME)
                    .Build();
            }
            else
            {
                req = GetAccountRequest.CreateBuilder()
                    .SetUserName(Username)
                    .SetSearchType(GetAccountRequest.Types.SearchType.ACCOUNTNAME)
                    .Build();
            }

            RcfProtoChannel channel = (RcfProtoChannel)RPCServiceStub.Channel;

            RPCServiceStub.GetAccount(null, req, null);

            GetAccountResponse res = (GetAccountResponse)channel.GetResponse();
            
            if (res != null)
            {
                List<AccountResponse> accounts = new List<AccountResponse>();
                foreach (var account in res.AccountsList)
                {
                    AccountResponse foundAccount = new AccountResponse()
                    {
                        account_id = account.AccountId,
                        active = account.Active,
                        username = account.UserName,
                        created = new DateTime(account.CreatedTime)
                    };
                    accounts.Add(foundAccount);

                    foundAccount.characters = new List<Models.Character.CharacterResponse>();

                    foreach (var character in account.CharactersList)
                    {
                        
                        var charResponse = new Models.Character.CharacterResponse() {
                             account_id = account.AccountId,
                             character_oid = character.ObjectId,
                             firstname = character.FirstName,
                             galaxy_id = character.GalaxyId,
                             galaxy_name = character.GalaxyName,
                             gender = character.Gender,
                             race = character.Race,
                             surname = character.SurName
                        };
                        if(character.HasBanned) {
                            charResponse.ban_expiration = new DateTime(character.BanExpiration);
                            charResponse.ban_reason = character.BanReason;
                            charResponse.banned = character.Banned;
                        }
                        foundAccount.characters.Add(charResponse);    
                    }
                }
                return accounts;
            }

            return null;

        }
    }
}