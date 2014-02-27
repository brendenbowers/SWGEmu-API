using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using OAuth2.DataModels;
using OAuth2.Server.Model;



namespace OAuth2.Server.Service
{
    public class AuthorizationAPI : ServiceStack.ServiceInterface.Service
    {

        public IApprovalModel ApprovalModel { get; set; }
        public IResourceOwnerModel ResourceOwnerModel { get; set; }
        public IClientModel ClientModel { get; set; }
        
        [Attributes.BearerTokenAuthenticate(RequireValidUser = true)]
        public List<ApprovalDetails> Get(ApprovalRequest Request)
        {
            if (!this.Request.Items.ContainsKey("auth:user"))
            {
                return null;
            }

            DataModels.Token token = this.Request.Items.GetValue<DataModels.Token>("auth:token");

            if (!token.scope.Contains("sjrb.oauth.authorizations"))
            {
                throw new TokenRequestError(ErrorCodes.invalid_scope, "sjrb.oauth.authorizations scope is required");
            }

            ResourceOwner user = this.Request.Items.GetValue<ResourceOwner>("auth:user");
            string[] memberof = user.GetValues<string>("memberOf");

            List<Approval> approvals = null;

            if (memberof != null && memberof.Contains("CN=NCC - Tool Support - RWS,OU=Security,OU=Mail Enabled,OU=Groups,OU=Corp,DC=SJRB,DC=AD"))
            {
                if (!string.IsNullOrWhiteSpace(Request.resource_owner_id))
                {
                    approvals = ApprovalModel.GetApprovalByResourceOwner(Request.resource_owner_id);
                }

                if (!string.IsNullOrWhiteSpace(Request.client_id))
                {
                    approvals = ApprovalModel.GetApprovalByClientID(Request.client_id);
                }
            }
            if (approvals == null)
            {
                approvals = ApprovalModel.GetApprovalByResourceOwner(user);
            }
            
            //Dictionary<string, ResourceOwner> owners = ResourceOwnerModel.GetByIDs(approvals.ConvertAll(cur => cur.resource_owner_id).Distinct()).ToDictionary(cur => cur.id);
            Dictionary<string, Client> clients = ClientModel.GetClients(approvals.ConvertAll(cur => cur.client_id).Distinct()).ToDictionary(cur => cur.id);

            return approvals.ConvertAll(toConvert => new ApprovalDetails
                {
                    client = clients[toConvert.client_id],
                    //resource_owner = owners[toConvert.resource_owner_id],
                    resource_owner_id = toConvert.resource_owner_id,
                    client_id = toConvert.client_id,
                    refresh_token = toConvert.refresh_token,
                    scope = toConvert.scope,
                    type = toConvert.type,
                }).ToList();
        }

        [Attributes.BearerTokenAuthenticate(RequireValidUser = true)]
        public void Delete(ApprovalRequest Request)
        {
            if (!this.Request.Items.ContainsKey("auth:user"))
            {
                return;
            }


            DataModels.Token token = this.Request.Items.GetValue<DataModels.Token>("auth:token");

            if (!token.scope.Contains("sjrb.oauth.authorizations"))
            {
                throw new TokenRequestError(ErrorCodes.invalid_scope, "sjrb.oauth.authorizations scope is required");
            }

            ResourceOwner user = this.Request.Items.GetValue<ResourceOwner>("auth:user");
            string[] memberof = user.GetValues<string>("memberOf");

            if(string.IsNullOrWhiteSpace(Request.client_id))
                throw new ArgumentException("client_id is required", "client_id");

            if (string.IsNullOrWhiteSpace(Request.resource_owner_id))
            {
                Request.resource_owner_id = user.id;
            }
                

            if (memberof != null && !memberof.Contains("CN=NCC - Tool Support - RWS,OU=Security,OU=Mail Enabled,OU=Groups,OU=Corp,DC=SJRB,DC=AD") 
                && Request.resource_owner_id != user.id)
            {
                throw new Exception("You do not have access to delete this approval");
            }

            if (!ApprovalModel.DeleteApproval(Request.client_id, Request.resource_owner_id))
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            
            
        } 
    }
}