using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using swgemurpcserver.rpc;
using SWGEmuAPI.Model.Account;
using OAuth2.DataModels;

namespace SWGEmuAPI.Service
{
    [OAuth2.Server.Attributes.BearerTokenAuthenticate]
    public class CharacterService : ServiceStack.ServiceInterface.Service
    {
        public Model.ICharacterModel CharacterModel { get; set; }

        public List<Model.Character.CharacterDetailsResponse> Get(Model.Character.CharacterDetailsRequest req)
        {
            List<Model.Character.CharacterDetailsResponse> res = null;
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
                var ro = Request.Items.GetValue<ResourceOwner>("auth:user");
                if (ro.GetSingle<string>("admin_level") != "0" && !res.TrueForAll(cur => cur.account_id == ro.id.ToUInt()))
                {
                    throw new ArgumentException("Requested character is not owned by the autorized account");
                }
                return res;
            }

            throw new ArgumentException("character_oid or firstname are required");
        }
    }
}