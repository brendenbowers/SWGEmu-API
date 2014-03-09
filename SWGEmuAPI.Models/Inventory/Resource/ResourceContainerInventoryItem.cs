using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model.Inventory.Resource
{
    public class ResourceContainerInventoryItem : CharacterInventoryItem
    {
        public string name { get; set; }
	    public string type { get; set; }
	    public ulong resource_id { get; set; }
	    public int count { get; set; }
	     
	    public List<ResourceSpawnClass> classes { get; set; }
	    public List<ResourceAttribute> attributes { get; set; }
    }
}
