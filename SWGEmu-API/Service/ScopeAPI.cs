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
    [Validator(typeof(Validators.ValidateScope))]
    public class ScopeAPI : ServiceStack.ServiceInterface.Service
    {
        public Model.IScopeModel ScopeModel { get; set; }

        [Attributes.BearerTokenAuthenticate(RequireValidUser = true)] 
        public IEnumerable<Scope> Get(Scope ToGet)
        {
            if (!this.Request.Items.ContainsKey("auth:user"))
            {
                return null;
            }

            ResourceOwner user = this.Request.Items.GetValue<ResourceOwner>("auth:user");
            

            if (string.IsNullOrWhiteSpace(ToGet.scope_name))
            {
                ///TODO:implement admin account check
                if (false)
                {
                    return ScopeModel.GetScopeDetails();
                }

                return ScopeModel.GetOwnedScopeDetails(user.id).ToList();
            }

            return ScopeModel.GetScopeDetails(ToGet.scope_name.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries));
        }

        [Attributes.BearerTokenAuthenticate(RequireValidUser = true)]
        public Scope Post(Scope ToCreate)
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

            if (ScopeModel.CreateScope(ToCreate) != null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Created;
                Response.StatusDescription = "Scope Created";
                return ToCreate;
            }

            throw new Exception("Unable to create scope");
        }

        [Attributes.BearerTokenAuthenticate(RequireValidUser = true)]
        public Scope Put(Scope ToUpdate)
        {
            if (!this.Request.Items.ContainsKey("auth:user"))
            {
                return null;
            }

            ResourceOwner user = this.Request.Items.GetValue<ResourceOwner>("auth:user");
            

            Scope scope = null;
            ///TODO:implement admin account check
            if (false)
            {
                scope = ScopeModel.SetScope(ToUpdate);
            }
            else
            {
                scope = ScopeModel.SetScope(ToUpdate, user);
            }

            if (scope != null)
            {
                return scope;
            }

            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            Response.StatusDescription = "You do not have access to update this scope";
            return null;
        }

        [Attributes.BearerTokenAuthenticate(RequireValidUser = true)]
        public Scope Patch(Scope ToUpdate)
        {
            if (!this.Request.Items.ContainsKey("auth:user"))
            {
                return null;
            }

            ResourceOwner user = this.Request.Items.GetValue<ResourceOwner>("auth:user");
           

            Scope scope = null;
            ///TODO:implement admin account check
            if (false)
            {
                scope = ScopeModel.UpdateScope(ToUpdate);
            }
            else
            {
                scope = ScopeModel.UpdateScope(ToUpdate, user);
            }

            if (scope != null)
            {
                return scope;
            }

            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            Response.StatusDescription = "You do not have access to update this scope";
            return null;
        }

        [Attributes.BearerTokenAuthenticate(RequireValidUser = true)]
        public object Delete(Scope ToDelete)
        {
            if (!this.Request.Items.ContainsKey("auth:user"))
            {
                return null;
            }

            ResourceOwner user = this.Request.Items.GetValue<ResourceOwner>("auth:user");

            ///TODO:implement admin account check
            bool deleted = false;
            if (false)
            {
                deleted = ScopeModel.DeleteScope(ToDelete);
            }
            else
            {
                deleted = ScopeModel.DeleteScope(ToDelete, user);
            }

            HttpResult res = new HttpResult();
            if (!deleted)
            {
                res.StatusCode = System.Net.HttpStatusCode.Forbidden;
                res.StatusDescription = "You do not have access to delete this scope";
            }

            return res;

        }
    }
}