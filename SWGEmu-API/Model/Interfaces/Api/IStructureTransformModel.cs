using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model
{
    public interface IStructureTransformModel
    {
        SWGEmuAPI.Model.Structure.StructureItemDetails TransformStructureDetails(swgemurpcserver.rpc.SWGEmuStructureItemDetails details);
        SWGEmuAPI.Model.Structure.StructureItem TransformStructure(swgemurpcserver.rpc.SWGEmuCharacterStructureItem charStructItem);
    }
}
