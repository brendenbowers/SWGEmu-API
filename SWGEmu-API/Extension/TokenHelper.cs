using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace OAuth2.Server.Extension
{
    public static class TokenHelper
    {
        public static string CreateAccessToken(int NumBytes = 44)
        {
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            byte[] tokenData = new byte[NumBytes];
            rng.GetBytes(tokenData);
            return System.Text.RegularExpressions.Regex.Replace(Convert.ToBase64String(tokenData), @"[\/\+\=]", "");
        }

        public static string ToScopeString(this IEnumerable<OAuth2.DataModels.Scope> Scopes, string Separator = " ")
        {

            if (Scopes == null)
                return "";

            int scopesCount = Scopes.Count();

            if(scopesCount <= 0) {
                return "";
            }

            if (scopesCount == 1)
            {
                return Scopes.First().scope_name;
            }
            
            
            StringBuilder bldr = new StringBuilder();
            for (int i = 0; i < scopesCount-1; i++)
            {
                bldr.Append(Scopes.ElementAt(i).scope_name + Separator);
            }

            bldr.Append(Scopes.ElementAt(scopesCount - 1));
            return bldr.ToString();
        }

        /// <summary>
        /// Returns a scope string of all scopes that exist in both scopes
        /// </summary>
        /// <param name="ScopeA">First Scopes</param>
        /// <param name="ScopeB">Second Scopes</param>
        /// <returns>The scopes that exist in both paramater scopes</returns>
        public static string IntersectScopes(string ScopeA, string ScopeB)
        {
            string[] res = IntersctScopesArray(ScopeA, ScopeB);
            if (res == null)
                return "";

            return string.Join(" ", res);
        }

        /// <summary>
        /// returns an array of scopes that exist in both scope paramaters
        /// </summary>
        /// <param name="ScopeA">First Scopes</param>
        /// <param name="ScopeB">Second Scopes</param>
        /// <returns>The scopes that exist in both paramater scopes</returns>
        public static string[] IntersctScopesArray(string ScopeA, string ScopeB)
        {
            if (string.IsNullOrWhiteSpace(ScopeA) || string.IsNullOrWhiteSpace(ScopeB))
            {
                return null;
            }

            string[] splitScopeA = ScopeA.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string[] splitScopeB = ScopeB.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            return splitScopeA.Intersect(splitScopeB).ToArray();
        }

        /// <summary>
        /// Returns an array of scopes from the first Scope set that are not present in the Second Scope set
        /// </summary>
        /// <param name="ScopeA">First Scopes</param>
        /// <param name="ScopeB">Second Scopes</param>
        /// <returns>any missing scopes found</returns>
        public static string[] MissingScopesArray(string ScopeA, string ScopeB)
        {
            if (ScopeA == null)
                ScopeA = "";
            if (ScopeB == null)
                ScopeB = "";

            string[] splitScopeA = ScopeA.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string[] splitScopeB = ScopeB.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            return splitScopeA.Where((cur) => !splitScopeB.Contains(cur)).ToArray();            
        } 
    }
}