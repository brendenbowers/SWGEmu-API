using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using swgemurpcserver.rpc;
using SWGEmuAPI.Models.Structure;
using OAuth2.DataModels;

namespace SWGEmuAPI.Service
{
    [OAuth2.Server.Attributes.BearerTokenAuthenticate]
    public class StrucutreService : ServiceStack.ServiceInterface.Service
    {
        public Model.StructureModel StructureModel { get; set; }

        public object Get(StructureRequest Req)
        {
            SWGEmuAPI.Models.Structure.StructureItemDetails details = StructureModel.GetStructureDetails(Req.object_id, Req.owner_object_id);
            var ro = Request.Items.GetValue<ResourceOwner>("auth:user");
            if (details != null && ro.id != "0" && ro.id.ToUInt() != details.owner_account_id)
            {
                throw new ArgumentException("Structure does not belong to the authorized user");
            }

            return details;
        }
    }
}