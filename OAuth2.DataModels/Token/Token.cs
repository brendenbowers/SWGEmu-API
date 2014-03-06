using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2.DataModels
{
    public class Token
    {
        public string access_token { get; set; }
        public TokenTypes token_type { get; set; }
        public long expires_in { get; set; }
        public string client_id { get; set; }
        public string resource_owner_id { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public long issue_time { get; set; }


        public virtual string ToURIString()
        {
            StringBuilder bldr = new StringBuilder();
            bldr.AppendFormat("access_token={0}&token_type={1}&expires_in={2}",access_token, token_type, expires_in);
            if (!string.IsNullOrWhiteSpace(scope))
            {
                bldr.AppendFormat("&scope={0}", scope.Replace(" ", "%20"));
            }

            if (refresh_token != null)
            {
                bldr.AppendFormat("&refresh_token={0}", refresh_token);
            }

            return bldr.ToString();
            
        }
    }
}
