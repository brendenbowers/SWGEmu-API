using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Models.Structure
{
    public class InstallationStructureItemDetails : StructureItemDetails
    {
	    public bool operating { get; set; }
	    public float actual_rate { get; set; }
	    public float max_hopper_size { get; set; }

        public List<object> hopper_items { get; set; }
    }
}
