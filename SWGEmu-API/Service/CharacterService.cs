using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using swgemurpcserver.rpc;
using SWGEmuAPI.Models.Account;

namespace SWGEmuAPI.Service
{
    [OAuth2.Server.Attributes.BearerTokenAuthenticate]
    public class CharacterService : ServiceStack.ServiceInterface.Service
    {
        public Model.CharacterModel CharacterModel { get; set; }

        public List<Models.Character.CharacterDetailsResponse> Get(Models.Character.CharacterDetailsRequest req)
        {
            List<Models.Character.CharacterDetailsResponse> res = null;
            if (req.character_oid != 0)
            {                
                res = CharacterModel.GetCharacter(req.character_oid, req.account_id);
            }
            if (!string.IsNullOrWhiteSpace(req.firstname))
            {
                res = CharacterModel.GetCharacter(req.firstname, req.account_id);
            }

            if (res.Count != 0)
            {
                if(!res.TrueForAll(cur => cur.account_id == this.Request.Items.GetValue<OAuth2.DataModels.ResourceOwner>("auth:user").GetSingle<string>("account_id").ToULong())) {
                    throw new ArgumentException("Requested character is not owned by the autorized account");
                }
                return res;
            }

            throw new ArgumentException("character_oid or firstname are required");
        }
    }
}