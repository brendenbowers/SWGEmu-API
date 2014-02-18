using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Models.Inventory.Pharmaceutical
{
    public class EnhancePackInventoryItem : PharmaceuticalInventoryItem
    {
    	public float duration { get; set; }
	    public string attribute { get; set; }
    }
}
