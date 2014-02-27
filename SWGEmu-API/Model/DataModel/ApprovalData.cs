using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OAuth2.DataModels;

namespace OAuth2.Server.Model.DataModel
{
    public class ApprovalData : OAuth2.Server.Model.DataModel.IApprovalData
    {
        public Client Client { get; set; }
        public ResourceOwner User { get; set; }
        public ResourceOwner Owner { get; set; }
        public List<Scope> RequestedScopes { get; set; }
        public string[] Errors { get; set; }
        public string Redirect { get; set; }
    }
}