using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model.Inventory.Armor
{
    public class ArmorInventoryItem : CharacterInventoryItem
    {
	    public float kinetic { get; set; }
	    public float energy { get; set; }
	    public float electricity { get; set; }
	    public float stun { get; set; }
	    public float blast { get; set; }
	    public float heat { get; set; }
	    public float cold { get; set; }
	    public float acid { get; set; }
	    public float lighsaber { get; set; }
	    public int health_encumberance { get; set; }
	    public int action_encumberance { get; set; }
	    public int mind_encumberance { get; set; }
	    public ArmorRating rating { get; set; }
        public List<int> hit_locations { get; set; }
    }
}
