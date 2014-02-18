using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Models.Inventory.Crafting
{
    public class CraftingComponentAttribute
    {
        public string id { get; set; }
        public string tite { get; set; }
        public int precision { get; set; }
        public float value { get; set; }
        public bool hidden { get; set; }
    }
}
