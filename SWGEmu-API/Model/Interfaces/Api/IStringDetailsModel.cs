using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWGEmuAPI.Model
{
    public interface IStringDetailsModel
    {
        string Get(string File, string Item);
        string Get(string ItemDetailsID);
    }
}