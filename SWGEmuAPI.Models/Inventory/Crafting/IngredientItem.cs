using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Models.Inventory.Crafting
{
    public class IngredientItem
    {
	    public int required_quantity { get; set; }
	    public bool identical { get; set; }
	    public string ingredient_slot_name { get; set; }
	    public CharacterInventoryItem ingredient { get; set; }
    }
}
