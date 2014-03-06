using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace OAuth2.DataModels
{
    [Route("/api/scope", "GET,POST,PUT,DELETE,PATCH")]
    [Route("/api/scope/{scope_name}", "GET,POST,PUT,DELETE,PATCH")]
    public class Scope
    {
        public string scope_name    { get; set; }
        public string description   { get; set; }
        public string owned_by      { get; set; }
    }
}
