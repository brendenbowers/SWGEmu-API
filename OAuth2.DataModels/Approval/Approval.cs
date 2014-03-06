using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2.DataModels
{
    public class Approval
    {
        public string client_id { get; set; }
        public string resource_owner_id { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
        public ApprovalTypes type { get; set; }
    }
}
