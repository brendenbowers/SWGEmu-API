using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model.Inventory.Crafting
{
    public class ManufacturingSchematicInventoryItem : CharacterInventoryItem
    {
	    public int manufacture_limit { get; set; }
	    public CharacterInventoryItem prototype_details { get; set; }
	    public List<BlueprintEntryItem> blueprint_entries { get; set; }
        public List<IngredientItem> ingredients { get; set; }
    }
}
