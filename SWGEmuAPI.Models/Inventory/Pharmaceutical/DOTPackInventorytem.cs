using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Models.Inventory.Pharmaceutical
{
    public class DOTPackInventorytem : PharmaceuticalInventoryItem
    {
    	public float potency { get; set; }
	    public uint duration { get; set; }
	    public string pool { get; set; }
	    public uint dot_type { get; set; }
	    public bool poison_unit { get; set; }
	    public bool disease_unit { get; set; }
    }   
}       
