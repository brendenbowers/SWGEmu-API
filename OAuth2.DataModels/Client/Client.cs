using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;

namespace OAuth2.DataModels
{
    [Route("/api/client", "GET,POST,PUT,DELETE,PATCH")]
    [Route("/api/client/{id}", "GET,POST,PUT,DELETE,PATCH")]    
    public class Client
    {        
        public string id { get; set; }
        public string name { get; set; }
        public ClientTypes type { get; set; }
        public string description { get; set; }
        public string secret { get; set; }
        public string owned_by { get; set; }
        public string allowed_scope { get; set; }
        public string icon { get; set; }
        public string redirect_uri { get; set; }
        public string contact_email { get; set; }
        public bool service_account { get; set; }
    }
}