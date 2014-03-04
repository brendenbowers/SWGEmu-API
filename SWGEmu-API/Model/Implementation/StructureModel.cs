using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeltaVSoft.RCFProto;
using swgemurpcserver.rpc;
using SWGEmuAPI.Models.Inventory;
using SWGEmuAPI.Models.Structure;

namespace SWGEmuAPI.Model
{
    public class StructureModel
    {
        public SWGEmuStructureItemDetailsService.Stub RPCServiceStub { get; set; }

        public StructureItemDetails GetStructureDetails(ulong objectID, ulong? ownerID)
        {
            GetStructureItemDetailsRequest req = null;
            if(ownerID.HasValue) {
                req = GetStructureItemDetailsRequest.CreateBuilder()
                    .SetObjectId(objectID)
                    .SetOwnerObjectId(ownerID.Value)
                    .Build();
            }
            else
            {
                req = GetStructureItemDetailsRequest.CreateBuilder()
                    .SetObjectId(objectID)
                    .Build();
            }

            
            RcfProtoChannel channel = (RcfProtoChannel)RPCServiceStub.Channel;

            RPCServiceStub.GetStructureItemDetails(null, req, null);

            GetStructureItemDetailsResponse res = (GetStructureItemDetailsResponse)channel.GetResponse();
            
            if (res != null)
            {
                var structDetails = res.StructuresList.FirstOrDefault();
                if (structDetails != null)
                {
                    return structDetails.ToStructureItemDetails();
                }
            }
            return null;
        }
    }
}