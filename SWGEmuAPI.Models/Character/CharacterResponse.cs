using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Models.Character
{
    public class CharacterResponse : CharacterDetailsRequest
    {
        public uint race { get; set; }
        public uint gender { get; set; }
        public string template { get; set; }
        public DateTime creation_date { get; set; }
        public bool banned { get; set; }
        public DateTime ban_expiration { get; set; }
        public string ban_reason { get; set; }

    }
}
