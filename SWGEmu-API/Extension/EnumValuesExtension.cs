using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuth2.Server.Extension
{
    public static class EnumValuesExtension<T>
        where T : struct
    {
        public static IEnumerable<T> GetValues()
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException("T is not an enum");
            }

            Array vals = Enum.GetValues(type);

            T[] enumerable = new T[vals.Length];
            Array.Copy(vals, enumerable, vals.Length);

            return enumerable;            
        }
    }
}