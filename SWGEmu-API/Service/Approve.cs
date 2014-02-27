using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;
using OAuth2.Server.Model.DataModel;
using OAuth2.Server.Extension;
using OAuth2.Server.Model;

namespace OAuth2.Server.Service
{
    [DefaultView("ApproveView")]
    public class Approve : ServiceStack.ServiceInterface.Service
    {
        public Model.IResourceOwnerModel ResourceOwnerModel { get; set; }
        public Model.IClientModel ClientModel { get; set; }
        public Model.IApprovalModel ApprovalModel { get; set; }
        public Model.IScopeModel ScopeModel { get; set; }

        public object Get(ApprovalRequest ApprovalRequest)
        {
            
            Uri redirectURI = null;
            Uri current = new Uri(Request.AbsoluteUri);
            ApprovalData data = new ApprovalData();
            
            if (!Uri.TryCreate(ApprovalRequest.redirect, UriKind.RelativeOrAbsolute, out redirectURI) || (redirectURI.IsAbsoluteUri && redirectURI.Host != current.Host))
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.invalid_request, "Invalid Redirect URI", data);
            }

            data.Redirect = ApprovalRequest.redirect;

            DataModels.ResourceOwner user = Session.Get<DataModels.ResourceOwner>("AuthResourceOwner");
            if (user == null)
            {
                UriBuilder bldr = new UriBuilder(Request.GetApplicationUrl());
                bldr.Path += "/auth/login";
                bldr.Query = "redirect=" + Request.AbsoluteUri.UrlEncode();

                return new HttpResult(data)
                {
                    Headers = { { "Location", bldr.ToString() } },
                    StatusCode = System.Net.HttpStatusCode.Redirect,
                };
            }
            data.User = user;

            DataModels.Client client = ClientModel.GetClientByID(ApprovalRequest.client_id);
            if (client == null)
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.invalid_request, "Invalid Client ID", data);            
            }

            if (!string.IsNullOrWhiteSpace(client.owned_by))
            {
                data.Owner = ResourceOwnerModel.GetByID(client.owned_by);
            }
            data.Client = client;

            string[] scopes = ApprovalRequest.scope == null ? new string[] {} : ApprovalRequest.scope.Split(new char[] { ' ', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<DataModels.Scope> scopeDetails = ScopeModel.GetScopeDetails(scopes).ToList();

            if(scopeDetails.Count != scopes.Length)
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.invalid_scope, "Invalid Scope(s) requested", data);
            }
            
            data.RequestedScopes = scopeDetails;

            return (IApprovalData)data;
        }

        public object Post(ApprovalResponse ApprovalResponse)
        {

            ApprovalData data = new ApprovalData();
            data.User = Session.Get<DataModels.ResourceOwner>("AuthResourceOwner");
            Request.Items.Add("Model", data);
            data.Redirect = ApprovalResponse.redirect;
            
            Uri referrerURI = Request.GetReferrerURI();
            Uri current = new Uri(Request.AbsoluteUri);
            
            //CRSF protection
            if (!referrerURI.SchemeHostPathMatch(current))
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.invalid_request, "Invalid Request", ApprovalResponse);
            }

            Uri redirectURI = null;
            if (!Uri.TryCreate(ApprovalResponse.redirect, UriKind.RelativeOrAbsolute, out redirectURI) || 
                (redirectURI.IsAbsoluteUri && redirectURI.Host != current.Host))
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.invalid_request, "Invalid Redirect URI", data);
            }

            data.Redirect = redirectURI.ToString();

            DataModels.ResourceOwner owner = Session.Get<DataModels.ResourceOwner>("AuthResourceOwner");
            if (owner == null)
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.access_denied, "Not Authenticated", data);            
            }

            data.Owner = owner;

            DataModels.Client client = ClientModel.GetClientByID(ApprovalResponse.client_id);
            if (client == null)
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.invalid_request, "Invalid Client ID", data); 
            }

            data.Client = client;

            List<DataModels.Scope> scopes = ScopeModel.GetScopeDetails(ApprovalResponse.approved_scopes).ToList();

            string scope = "";
            if (scopes != null)
            {
                scopes.ForEach((cur) => scope += cur.scope_name + " ");
            }
            
            data.RequestedScopes = scopes;

            DataModels.Approval approval = new DataModels.Approval()
            {
                client_id = client.id,
                resource_owner_id = owner.id,
                type = DataModels.ApprovalTypes.user_granted,
                scope = scope,
            };


            if (!ApprovalModel.AddOrUpdateApproval(approval))
            {
                throw TokenErrorUtility.CreateError(DataModels.ErrorCodes.server_error, "Error storing approval", data);
            }

            return new HttpResult(data)
            {
                StatusCode = System.Net.HttpStatusCode.Redirect,
                Location = ApprovalResponse.redirect
            };
        }
    }
}