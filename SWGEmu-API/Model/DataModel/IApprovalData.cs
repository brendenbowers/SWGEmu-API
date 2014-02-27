using System;
namespace OAuth2.Server.Model.DataModel
{
    public interface IApprovalData
    {
        OAuth2.DataModels.Client Client { get; set; }
        OAuth2.DataModels.ResourceOwner User { get; set; }
        OAuth2.DataModels.ResourceOwner Owner { get; set; }
        System.Collections.Generic.List<OAuth2.DataModels.Scope> RequestedScopes { get; set; }
        string[] Errors { get; set; }
        string Redirect { get; set; }
    }
}
