using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OAuth2.DataModels;
using SWGEmuAPI.Models.Account;

namespace OAuth2.Server.Extension
{
    public static class IResourceOwnerModelExtensions
    {

        public static OAuth2.DataModels.ResourceOwner CreateOrUpdateFromAccountModel(this OAuth2.Server.Model.IResourceOwnerModel Model, AccountResponse Account)
        {
            OAuth2.DataModels.ResourceOwner owner = new DataModels.ResourceOwner()
            {
                id = Account.account_id.ToString(),
                 time = DateTime.UtcNow.GetTotalSeconds(),
                 attributes = Account.ToDictonary(),
            };

            if (Model.CreateOrUpdate(owner))
            {
                return owner;
            }

            return null;
        }
    }
}