using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using swgemurpcserver.rpc;
using SWGEmuAPI.Models.Structure;

namespace SWGEmuAPI.Service
{
    public class StrucutreService : ServiceStack.ServiceInterface.Service
    {
        public Model.StructureModel StructureModel { get; set; }

        public object Get(StructureRequest Req)
        {
            return StructureModel.GetStructureDetails(Req.object_id, Req.owner_object_id);
        }
    }
}