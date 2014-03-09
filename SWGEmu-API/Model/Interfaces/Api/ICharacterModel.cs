using SWGEmuAPI.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Model
{
    public interface ICharacterModel
    {
        List<CharacterDetailsResponse> GetCharacter(string characterName, uint accountID);
        List<CharacterDetailsResponse> GetCharacter(ulong characterOID, uint accountID);
    }
}
