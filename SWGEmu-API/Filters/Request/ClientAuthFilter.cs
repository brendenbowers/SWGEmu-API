using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using ServiceStack.ServiceHost;
using OAuth2.Server.Model.DataModel;
using OAuth2.DataModels;

namespace OAuth2.Server.Filters.Request
{
    public class ClientAuthFilter : IHasRequestFilter
    {

        public int Priority
        {
            get;
            set;
        }

        public ClientAuthFilter()
        {
            Priority = 1;
        }

        public IHasRequestFilter Copy()
        {
            return new ClientAuthFilter() { Priority = Priority };
        }

        public void RequestFilter(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            ITokenRequest request = requestDto as ITokenRequest;

            if (request == null)
            {
                return;
            }

            KeyValuePair<string, string>? clientdetails = req.GetBasicAuthUserAndPassword();

            if (clientdetails != null)
            {
                request.client_id = clientdetails.Value.Key;
                request.client_password = clientdetails.Value.Value;
            }
        }
    }
}