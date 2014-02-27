using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace SWGEmuAPI.Models.Galaxy
{
    [Route("/galaxy",Verbs="GET")]
    [Route("/galaxy/{galaxy_id}", Verbs = "GET")]
    public class GalaxyRequest
    {
        public int galaxy_id { get; set; }
        public string name { get; set; }
    }
}
