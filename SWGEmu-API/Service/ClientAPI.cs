using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OAuth2.DataModels;
using ServiceStack.ServiceHost;
using ServiceStack.Common.Web;
using ServiceStack.FluentValidation.Attributes;

namespace OAuth2.Server.Service
{
    [Validator(typeof(Validators.ValidateClient))]
    public class ClientAPI : ServiceStack.ServiceInterface.Service
    {
        public Model.IClientModel ClientModel { get; set; }

        [Attributes.BearerTokenAuthenticate(RequireValidUser=true)]        
        public List<Client> Get(Client Request)
        {

            if (!this.Request.Items.ContainsKey("auth:user"))
            {
                return null;
            }

            ResourceOwner user = this.Request.Items.GetValue<ResourceOwner>("auth:user");

            string[] memberof = user.GetValues<string>("memberOf");

            if (!string.IsNullOrWhiteSpace(Request.id))
            {
                Client foundClient = ClientModel.GetClientByID(Request.id);
                ///TODO:implement admin account check
                if (foundClient != null && (foundClient.owned_by == user.id))
                {
                    return new List<Client>() { foundClient };
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    Response.StatusDescription = "You do not have access to this client";
                    throw new Exception("You do not have access to this client");
                }
            }

            ///TODO:implement admin account check
            if (false)
            {
                return ClientModel.GetClients();
            }

            return ClientModel.GetClientsByOwner(user.id);

        }

        [Attributes.BearerTokenAuthenticate(RequireValidUser = true)]        
        public Client Post(Client ToCreate)
        {

            if (!this.Request.Items.ContainsKey("auth:user"))
            {
                return null;
            }

            ResourceOwner user = this.Request.Items.GetValue<ResourceOwner>("auth:user");

            if (string.IsNullOrWhiteSpace(ToCreate.owned_by))
            {
                ToCreate.owned_by = user.id;
            }

            if (ClientModel.CreateClient(ToCreate))
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Created;
                Response.StatusDescription = "Client Created";
                return ToCreate;
            }

            throw new Exception("Unknown error when storing client");
        }

        [Attributes.BearerTokenAuthenticate(RequireValidUser = true)] 
        public Client Put(Client ToUpdate)
        {
            Client updated = null;
            if (!this.Request.Items.ContainsKey("auth:user"))
            {
                return null;
            }

            ResourceOwner user = this.Request.Items.GetValue<ResourceOwner>("auth:user");
            ///TODO:implement admin account check
            if (false)
            {
                updated = ClientModel.SetClient(ToUpdate);
            }
            else
            {
                updated = ClientModel.SetClient(ToUpdate, user);
            }

            if(updated == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                Response.StatusDescription = "You do not have access to this client";
            }

            return updated;
        }

        [Attributes.BearerTokenAuthenticate(RequireValidUser = true)] 
        public Client Patch(Client ToUpdate)
        {
            Client updated = null;
            if (!this.Request.Items.ContainsKey("auth:user"))
            {
                return null;
            }

            ResourceOwner user = this.Request.Items.GetValue<ResourceOwner>("auth:user");
            ///TODO:implement admin account check
            if (false)
            {
                updated = ClientModel.UpdateClient(ToUpdate);
            }
            else
            {
                updated = ClientModel.UpdateClient(ToUpdate, user);
            }

            if (updated == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                Response.StatusDescription = "You do not have access to this client";
            }

            return updated;
        }

        [Attributes.BearerTokenAuthenticate(RequireValidUser = true)] 
        public object Delete(Client ToDelete)
        {
            bool deleted = false;
            if (!this.Request.Items.ContainsKey("auth:user"))
            {
                return null;
            }

            ResourceOwner user = this.Request.Items.GetValue<ResourceOwner>("auth:user");
            ///TODO:implement admin account check
            if (false)
            {
                deleted = ClientModel.DeleteClient(ToDelete);
            }
            else
            {
                deleted = ClientModel.DeleteClient(ToDelete, user);
            }

            HttpResult res = new HttpResult()
            {
                StatusDescription = "Client Deleted",
            }; 

            if (!deleted)
            {
                res.StatusCode = System.Net.HttpStatusCode.Forbidden;
                res.StatusDescription = "You do not have access to this client";
            }

            return res;
        }
    }
}