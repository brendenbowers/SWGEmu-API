using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2.DataModels
{
    public class ResourceOwner
    {
        public string id { get; set; }
        public long time { get; set; }
        public Dictionary<string, object[]> attributes { get; set; }

        /// <summary>
        /// Gets a single string value from the attribute collection of a resource owner
        /// </summary>
        /// <param name="PropertyName">Name of the attribute to get</param>
        /// <returns>The attribute found</returns>
        public string GetSingle(string PropertyName)
        {
            return GetSingle<string>(PropertyName);
        }

        /// <summary>
        /// Gets a single string value from the attribute collection of a resource owner
        /// </summary>
        /// <typeparam name="T">The type of attribute to get</typeparam>
        /// <param name="PropertyName">The name of the attribute to get</param>
        /// <returns>The found attribute</returns>
        public T GetSingle<T>(string PropertyName)
        {
            object[] vals = null;
            if (attributes.TryGetValue(PropertyName, out vals))
            {
                return (T)vals.FirstOrDefault();
            }
            return default(T);
        }

        /// <summary>
        /// Gets all values for an attribute
        /// </summary>
        /// <typeparam name="T">The type of attribute to get</typeparam>
        /// <param name="PropertyName">The attribute to get</param>
        /// <returns>The found attributes</returns>
        public T[] GetValues<T>(string PropertyName)
        {
            object[] vals = null;
            if(attributes.TryGetValue(PropertyName, out vals))
            {
                return vals.Cast<T>().ToArray();
            }
            return null;
        }
    }
}
