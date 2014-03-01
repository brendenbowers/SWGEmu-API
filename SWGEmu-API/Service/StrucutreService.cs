using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using swgemurpcserver.rpc;
using SWGEmuAPI.Models.Structure;

namespace SWGEmuAPI.Service
{
    [OAuth2.Server.Attributes.BearerTokenAuthenticate]
    public class StrucutreService : ServiceStack.ServiceInterface.Service
    {
        public Model.StructureModel StructureModel { get; set; }

        public object Get(StructureRequest Req)
        {
            SWGEmuAPI.Models.Structure.StructureItemDetails details = StructureModel.GetStructureDetails(Req.object_id, Req.owner_object_id);

            if (details != null && !this.Request.Items.GetValue<OAuth2.DataModels.ResourceOwner>("auth:user")
                                        .GetValues<SWGEmuAPI.Models.Character.CharacterResponse>("characters")
                                        .Any(character => character.firstname == details.owner_display_name))
            {
                throw new ArgumentException("Structure does not belong to the authorized user");
            }

            return details;
        }
    }
}