using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;


namespace SWGEmuAPI.Model.Structure
{
    [Route("/structures",Verbs="GET")]    
    public class StructureRequest
    {
        public ulong object_id { get; set; }
        public ulong? owner_object_id { get; set; }
    }
}
