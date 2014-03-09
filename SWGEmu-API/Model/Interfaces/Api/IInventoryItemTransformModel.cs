using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model
{
    public interface IInventoryItemTransformModel
    {
        Model.Inventory.CharacterInventoryItem TransformInventoryItem(swgemurpcserver.rpc.CharacterInventoryItem inventoryItem);
    }
}
