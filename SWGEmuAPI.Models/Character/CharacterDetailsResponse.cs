using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model.Character
{
    public class CharacterDetailsResponse
    {
        public ulong character_oid { get; set; }
        public uint account_id { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public uint bank_credits { get; set; }
        public uint cash_credits { get; set; }
        public uint base_health { get; set; }
        public uint base_action { get; set; }
        public uint base_mind { get; set; }
        public string biography { get; set; }
        public int max_lots { get; set; }
        public int remaining_lots { get; set; }
        public string title { get; set; }
        public string appearance_file { get; set; }
        public List<object> Inventory { get; set; }
        public List<Structure.StructureItem> Structures { get; set; }
    }
}