using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2.DataModels
{
    public class TokenInfo : Token
    {
        public ResourceOwner owner { get; set; }
    }
}
