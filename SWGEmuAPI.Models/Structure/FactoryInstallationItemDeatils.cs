using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWGEmuAPI.Models.Inventory;

namespace SWGEmuAPI.Models.Structure
{
    public class FactoryInstallationItemDeatils : InstallationStructureItemDetails
    {
        public object shcematic { get; set; }
        public List<object> ingredient_items { get; set; }
    }
}
