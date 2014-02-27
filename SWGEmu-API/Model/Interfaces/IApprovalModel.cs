using System;
using System.Collections.Generic;
using OAuth2.DataModels;



namespace OAuth2.Server.Model
{
    public interface IApprovalModel
    {
        bool AddOrUpdateApproval(OAuth2.DataModels.Approval Approval);
        List<Approval> GetApprovalByResourceOwner(string ResourceOwnerID);
        List<Approval> GetApprovalByResourceOwner(ResourceOwner ResourceOwner);
        List<Approval> GetApprovalByClientID(Client Client);
        List<Approval> GetApprovalByClientID(string ClientID);
        OAuth2.DataModels.Approval GetApproval(OAuth2.DataModels.Client Client, OAuth2.DataModels.ResourceOwner ResourceOwner);
        OAuth2.DataModels.Approval GetApproval(string ClientID, string ResourceOwnerID);
        OAuth2.DataModels.Approval GetApprovalByRefreshToken(string ClientID, string RefreshToken);
        OAuth2.DataModels.Approval GetApprovalByRefreshToken(OAuth2.DataModels.Client Client, string RefreshToken);
        bool DeleteApproval(string ClientID, string ResourceOwnerID);
        bool DeleteApproval(Approval ToDelete);
        bool DeleteApproval(Client Client, ResourceOwner ResourceOwner);
    }
}
