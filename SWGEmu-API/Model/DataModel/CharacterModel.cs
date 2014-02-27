using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using swgemurpcserver.rpc;
using SWGEmuAPI.Models.Character;
using SWGEmuAPI.Models.Inventory;
using SWGEmuAPI.Models.Structure;
using DeltaVSoft.RCFProto;

namespace SWGEmuAPI.Model
{
    public class CharacterModel
    {
        public SWGEmuCharacterDetailsService.Stub RPCServiceStub { get; set; }

        public List<CharacterDetailsResponse> GetCharacter(string characterName, uint accountID)
        {
            GetCharacterDetailsRequest req = GetCharacterDetailsRequest.CreateBuilder()
                .SetFirstName(characterName)
                .SetAccountId(accountID)
                .Build();

            return MakeRPCRequest(req);

        }

        public List<CharacterDetailsResponse> GetCharacter(ulong characterOID, uint accountID)
        {
            GetCharacterDetailsRequest req = GetCharacterDetailsRequest.CreateBuilder()
                .SetObjectId(characterOID)
                .SetAccountId(accountID)
                .Build();

            return MakeRPCRequest(req);
        }


        protected List<CharacterDetailsResponse> MakeRPCRequest(GetCharacterDetailsRequest req)
        {
            RcfProtoChannel channel = (RcfProtoChannel)RPCServiceStub.Channel;
            
            RPCServiceStub.GetCharacterDetails(null, req,null);

            GetCharacterDetailsResponse res = (GetCharacterDetailsResponse)channel.GetResponse();     

            if (res == null)
            {
                return null;
            }

            List<CharacterDetailsResponse> characters = new List<CharacterDetailsResponse>();
            foreach (var character in res.CharacterDetailsList)
            {
                var charDetails = new CharacterDetailsResponse()
                {
                    account_id = character.AccountId,
                    bank_credits = character.BankCredits,
                    base_action = character.BaseAction,
                    base_health = character.BaseHealth,
                    base_mind = character.BaseMind,
                    cash_credits = character.CashCredits,
                    character_oid = character.ObjectId,
                    firstname = character.FirstName,
                    surname = character.SurName,
                };

                if (character.InventoryItemsList.Count > 0)
                {
                    charDetails.Inventory = character.InventoryItemsList
                        .ToList()
                        .ConvertAll<object>(cur => cur.ToInventoryItem())
                        .ToList();
                }

                if (character.StructuresList.Count > 0)
                {
                    charDetails.Structures = character.StructuresList.ToList()
                        .ConvertAll<StructureItem>(cur => cur.ToStructureItem()).ToList();
                }

                if (character.HasBiography)
                {
                    charDetails.biography = character.Biography;
                }

                if (character.HasTitle)
                {
                    charDetails.title = character.Title;
                }

                if (character.HasRemainingPlots)
                {
                    charDetails.remaining_lots = character.RemainingPlots;
                }

                if (character.HasMaximumimPlots)
                {
                    charDetails.max_lots = character.MaximumimPlots;
                }

                if (character.HasAppearanceFile)
                {
                    charDetails.appearance_file = character.AppearanceFile;
                }

                characters.Add(charDetails);
            }
            return characters;
        }
    }
}