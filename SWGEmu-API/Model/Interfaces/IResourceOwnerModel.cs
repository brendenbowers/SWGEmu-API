using System;
using System.Collections.Generic;
using OAuth2.DataModels;



namespace OAuth2.Server.Model
{
    public interface IResourceOwnerModel
    {
        bool Create(global::OAuth2.DataModels.ResourceOwner Owner);
        bool CreateOrUpdate(global::OAuth2.DataModels.ResourceOwner Owner);
        global::OAuth2.DataModels.ResourceOwner GetByID(string ID);
        bool Update(global::OAuth2.DataModels.ResourceOwner Owner);
        List<ResourceOwner> GetByIDs(IEnumerable<string> IDs);
    }
}
