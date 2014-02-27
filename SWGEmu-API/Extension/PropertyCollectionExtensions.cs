using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuth2.Server.Extension
{
    public static class PropertyCollectionExtensions
    {
        public static Dictionary<string, object> ToDictonary(this System.DirectoryServices.PropertyCollection Collection, string[] ToAdd = null)
        {
            if (Collection == null)
                throw new ArgumentNullException("Collection", "DirectoryServices PropertyCollection is null");

            Dictionary<string, object> values = new Dictionary<string, object>(Collection.Count);

            IEnumerable<string> propertynames;


            if (ToAdd != null)
            {
                propertynames = from name in (new PropertyNameCollectionEnumerable(Collection.PropertyNames))
                                where ToAdd.Contains(name)
                                select name;
            }
            else
            {
                propertynames = new PropertyNameCollectionEnumerable(Collection.PropertyNames);
            }

            foreach (string name in propertynames)
            {
                values.Add(name, Collection[name].Value);
            }

            return values;
        }


        private class PropertyNameCollectionEnumerable : IEnumerable<string>
        {
            protected System.Collections.ICollection _ToEnumerate = null;


            public PropertyNameCollectionEnumerable(System.Collections.ICollection PropertyNames)
            {
                _ToEnumerate = PropertyNames;
            }

            public IEnumerator<string> GetEnumerator()
            {
                return new PropertyNameCollectionEnumerator(_ToEnumerate);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new PropertyNameCollectionEnumerator(_ToEnumerate);
            }
        }

        private class PropertyNameCollectionEnumerator : IEnumerator<string>
        {

            public PropertyNameCollectionEnumerator(System.Collections.ICollection PropertyNames)
            {
                _Enumerator = PropertyNames.GetEnumerator();
            }
            
            protected System.Collections.IEnumerator _Enumerator = null;

            public string Current
            {
                get { return (string)_Enumerator.Current; }
            }

            public void Dispose()
            {
                if (_Enumerator == null)
                    return;

                IDisposable enumerator = _Enumerator as IDisposable;
                if (enumerator != null)
                {
                    enumerator.Dispose();
                }

                _Enumerator = null;
            }

            object System.Collections.IEnumerator.Current
            {
                get { return _Enumerator.Current; }
            }

            public bool MoveNext()
            {
                return _Enumerator.MoveNext();
            }

            public void Reset()
            {
                _Enumerator.Reset();
            }
        }
    }
}