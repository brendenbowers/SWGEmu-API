using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Models.Structure
{
    public class StructureItem
    {
        public ulong object_id { get; set; }
        public string appearance_file_name { get; set; }
        public string object_name { get; set; }
        public string display_name { get; set; }
        public string resolved_object_name { get; set; }
        public string template_file_name { get; set; }
        public string portals_file_name { get; set; }
    }
}
