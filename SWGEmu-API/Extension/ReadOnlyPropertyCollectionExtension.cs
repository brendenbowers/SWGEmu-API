using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;

namespace System.DirectoryServices
{
    public static class ReadOnlyPropertyCollectionExtension
    {
        public static T SingleValue<T>(this ResultPropertyCollection col, string Property)
        {
            if (!col.Contains(Property))
            {
                return default(T);
            }

            ResultPropertyValueCollection valueCol = col[Property];
            if (valueCol.Count == 0)
                return default(T);
            return (T)valueCol[0];
        }
    }
}