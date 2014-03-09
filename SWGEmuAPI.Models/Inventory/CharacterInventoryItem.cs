using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model.Inventory
{
    public class CharacterInventoryItem
    {
        public ulong object_id { get; set; }
        public string appearance_file_name { get; set; }
        public string template_file_name { get; set; }
        public string portals_file_name { get; set; }
        public string object_name { get; set; }
        public string display_name { get; set; }
        public string description { get; set; }
        public string crafter_name { get; set; }
        public string serial_number { get; set; }
        public int max_condition { get; set; }
        public int condition { get; set; }

    }
}
