using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model.Inventory.Pharmaceutical
{
    public class PharmaceuticalInventoryItem : CharacterInventoryItem
    {
	     public PharmaceuticalType pharma_type { get; set; }
	     public float effectiveness { get; set; }
	     public float area { get; set; }
	     public int medicine_required { get; set; }
         public int use_count { get; set; }
    }
}
