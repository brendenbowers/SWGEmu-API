using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2.DataModels
{
    public class TokenResponse : Token
    {
        public string state { get; set; }

        public override string ToURIString()
        {

            if (state != null)
            {
                return base.ToURIString() + "&state=" + state;
            }

            return base.ToURIString();
        }
    }
}
