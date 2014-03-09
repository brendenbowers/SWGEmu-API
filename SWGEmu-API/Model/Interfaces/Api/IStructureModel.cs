using SWGEmuAPI.Model.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model
{
    public interface IStructureModel
    {
        StructureItemDetails GetStructureDetails(ulong objectID, ulong? ownerID);
    }
}
