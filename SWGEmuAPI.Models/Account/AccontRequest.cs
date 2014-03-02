using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace SWGEmuAPI.Models.Account
{
    [Route("/account",Verbs="GET")]
    //[Route("/account/{account_id}", Verbs = "GET")]
    public class AccontRequest
    {
        public string username { get; set; }
        public uint account_id { get; set; }
        public string password { get; set; }
    }
}
