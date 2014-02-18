using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace SWGEmuAPI.Model
{
    public class StringDetailsModel
    {

        private static Regex MATCHSTRINGID = new Regex(@"@(?<filename>\w+):(?<id>\w+)", RegexOptions.Compiled);

        private ConcurrentDictionary<string, StringDetailsFile> _fileDetails = new ConcurrentDictionary<string, StringDetailsFile>();
        private ServiceStack.IO.IVirtualDirectory _virtualDir;

        public StringDetailsModel(ServiceStack.IO.IVirtualDirectory Dir)
        {
            _virtualDir = Dir;
        }

        private StringDetailsFile LoadFile(string File) 
        {
            if (!File.EndsWith(".csv"))
            {
                File += ".csv";
            }

            if (_virtualDir == null)
            {
                throw new DirectoryNotFoundException("base directory for details not found");
            }

            var foundFile = _virtualDir.GetFile(File);
            if (foundFile == null)
            {
                throw new FileNotFoundException("details file does not exist: " + File);
            }

            return new StringDetailsFile(foundFile);            
        }

        /// <summary>
        /// Gets the details string from the provided file for the item with the provided id
        /// </summary>
        /// <param name="File"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public string this[string File, string Item] {
            get
            {
                return Get(File, Item);
            }
        }

        /// <summary>
        /// Gets the details string from the provided file for the item with the provided id
        /// </summary>
        /// <param name="File"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public string Get(string File, string Item)
        {
            if (File.EndsWith(".csv"))
            {
                File = File.Substring(0, File.Length - 4);
            }

            return _fileDetails.GetOrAdd(File, LoadFile).Get(Item);
        }
        /// <summary>
        /// Item Details for the id in format of @{File}:Item
        /// </summary>
        /// <param name="ItemDetailsID">@{File}:Item</param>
        /// <returns>The details string or null if not found</returns>
        public string this[string ItemDetailsID]
        {
            get
            {
                return Get(ItemDetailsID);
            }
        }

        /// <summary>
        /// Item Details for the id in format of @{File}:Item
        /// </summary>
        /// <param name="ItemDetailsID">@{File}:Item</param>
        /// <returns>The details string or null if not found</returns>
        public string Get(string ItemDetailsID)
        {

            var match = MATCHSTRINGID.Match(ItemDetailsID);


            if(match.Success) {
                string details = Get(match.Groups["filename"].Value, match.Groups["id"].Value);
                if (string.IsNullOrWhiteSpace(details))
                {
                    return ItemDetailsID;
                }

                return details;
            }
            return ItemDetailsID;
        }

        

        protected class StringDetailsFile
        {
            private ConcurrentDictionary<string, string> _Map = new ConcurrentDictionary<string, string>();

            public StringDetailsFile(string path)
            {
                ProcessFileLines(File.ReadAllLines(path));                
            }

            public StringDetailsFile(ServiceStack.IO.IVirtualFile File)
            {
                ProcessFileLines(File.ReadAllText().Split(new char[] {'\r','\n'}, StringSplitOptions.RemoveEmptyEntries));
            }

            protected void ProcessFileLines(string[] Lines)
            {
                foreach (string line in Lines)
                {
                    string[] split = line.Split(new char[] { ',' }, 3, StringSplitOptions.RemoveEmptyEntries);
                    if (split.Length == 3)
                    {
                        _Map.AddOrUpdate(split[1], split[2],(existing,toAdd) => toAdd);
                    }
                }
            }

            public string this[string Item]
            {
                get
                {
                    return Get(Item);
                }
            }

            public string Get(string Item)
            {
                string val = string.Empty;
                _Map.TryGetValue(Item, out val);
                return val;
            }
        }
    }
}