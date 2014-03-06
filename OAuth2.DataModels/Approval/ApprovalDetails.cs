using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2.DataModels
{
    public class ApprovalDetails : Approval
    {
        public ResourceOwner resource_owner { get; set; }
        public Client client { get; set; }
    }
}
