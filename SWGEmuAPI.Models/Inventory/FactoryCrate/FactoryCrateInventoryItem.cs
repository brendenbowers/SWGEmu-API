using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Models.Inventory.FactoryCrate
{
    public class FactoryCrateInventoryItem : CharacterInventoryItem
    {
        public int count { get; set; }
	    public int max_items { get; set; }
	    public object contained_items { get; set; }
    }
}
