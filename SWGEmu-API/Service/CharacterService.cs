using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using swgemurpcserver.rpc;
using SWGEmuAPI.Models.Account;

namespace SWGEmuAPI.Service
{
    public class CharacterService : ServiceStack.ServiceInterface.Service
    {
        public Model.CharacterModel CharacterModel { get; set; }

        public List<Models.Character.CharacterDetailsResponse> Get(Models.Character.CharacterDetailsRequest req)
        {

            if (req.character_oid != 0)
            {
                return CharacterModel.GetCharacter(req.character_oid, req.account_id);
            }
            if (!string.IsNullOrWhiteSpace(req.firstname))
            {
                return CharacterModel.GetCharacter(req.firstname, req.account_id);
            }

            throw new ArgumentException("character_oid or firstname are required");
        }
    }
}