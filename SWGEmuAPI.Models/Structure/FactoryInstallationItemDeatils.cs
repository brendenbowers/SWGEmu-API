using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWGEmuAPI.Model.Inventory;

namespace SWGEmuAPI.Model.Structure
{
    public class FactoryInstallationItemDeatils : InstallationStructureItemDetails
    {
        public object shcematic { get; set; }
        public List<object> ingredient_items { get; set; }
    }
}
