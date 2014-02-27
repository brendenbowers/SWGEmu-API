using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Models.Account
{
    public class AccountResponse : AccontRequest
    {
        public DateTime created { get; set; }
        public bool active { get; set; }
        public List<Character.CharacterResponse> characters { get; set; }
    }
}
