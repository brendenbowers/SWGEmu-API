using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Text;

namespace System.Collections.Generic
{
    public static class IDictonaryExtension
    {
        public static T GetValue<T>(this IDictionary<string,object> Dictonary, string Key, bool ThrowException = false)
        {
            return (T)Dictonary.GetValue<string, object>(Key, ThrowException);
        }

        public static TVal GetValue<TKey,TVal>(this IDictionary<TKey,TVal> Dictonary, TKey Key, bool ThrowException =false)
        {
            TVal val = default(TVal);
            if(!Dictonary.TryGetValue(Key, out val) && ThrowException)
            {
                throw new KeyNotFoundException("value with key {0} does not exist in the dictonary".Fmt(Key));
            }

            return val;
        }
    }
}