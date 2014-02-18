using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Models.Structure
{
    public class StructureItemDetails : StructureItem
    {

	    public string owner_display_name { get; set; }
	    public float world_x { get; set; }
	    public float world_y { get; set; }
	    public float world_z { get; set; }
	    public string zone { get; set; }
	    public int power { get; set; }
	    public int maintenance { get; set; }
	    public int decay_percent{ get; set; }
	    public int lot_size { get; set; }
    }
}
