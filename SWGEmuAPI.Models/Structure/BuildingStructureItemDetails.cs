using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Models.Structure
{
    public class BuildingStructureItemDetails : StructureItemDetails
    {
        public List<object> contained_items { get; set; }
    }
}
