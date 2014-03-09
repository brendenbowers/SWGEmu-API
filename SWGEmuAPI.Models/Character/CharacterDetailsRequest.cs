using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace SWGEmuAPI.Model.Character
{
    [Route("/account/{account_id}/characters", Verbs = "GET")]
    [Route("/account/characters", Verbs = "GET")]
    [Route("/account/{account_id}/characters/{character_oid}", Verbs = "GET")]
    [Route("/characters", Verbs="GET")]
    public class CharacterDetailsRequest
    {
        public uint account_id { get; set; }
        public ulong character_oid { get; set; }
        public uint galaxy_id { get; set; }
        public string galaxy_name { get; set; }
        public string surname { get; set; }
        public string firstname { get; set; }
    }
}
