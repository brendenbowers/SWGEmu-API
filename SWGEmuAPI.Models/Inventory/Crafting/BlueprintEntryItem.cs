using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model.Inventory.Crafting
{
    public class BlueprintEntryItem
    {
        public string type { get; set; }
	    public string key { get; set; }
	    public string serial { get; set; }
	    public string display_name { get; set; }
	    public int quantity { get; set; }
	    public bool identical { get; set; }
    }
}
