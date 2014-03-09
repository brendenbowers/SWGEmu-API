using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model.Inventory.Pharmaceutical
{
    public class RevivePackInventoryItem : PharmaceuticalInventoryItem
    {
	    public float health_wound_healed { get; set; }
	    public float health_healed { get; set; }
	    public float action_wound_healed { get; set; }
	    public float action_healed { get; set; }
	    public float mind_wound_healed { get; set; }
	    public float mind_healed { get; set; }
    }
}
