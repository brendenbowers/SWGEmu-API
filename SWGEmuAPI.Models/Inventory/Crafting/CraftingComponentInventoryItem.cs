using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model.Inventory.Crafting
{
    public class CraftingComponentInventoryItem : CharacterInventoryItem
    {
        public List<CraftingComponentAttribute> attributes { get; set; }
    }
}
