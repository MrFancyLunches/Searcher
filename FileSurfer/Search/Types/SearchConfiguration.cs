using FileSurfer.Search.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearcher.Search.Types
{
    [Serializable]
    public class SearchConfiguration
    {
        public List<string> Extensions { get; private set; } = new List<string>();

        public List<SearchKey> OrKeys { get; private set; } = new List<SearchKey>();

        public List<SearchKey> AndKeys { get; private set; } = new List<SearchKey>();

        public List<SearchKey> NotKeys { get; private set; } = new List<SearchKey>();

        public List<string> BaseDirectories { get; private set; } = new List<string>();

        public DateTimeRange ModifiedTime { get; private set; } = null;

        public DateTimeRange CreatedTime { get; private set; } = null;

        [JsonIgnore]
        public List<Func<bool>> Conditions { get; private set; } = null;

        public bool Recursive { get; set; } = false;

        public DateTime TimeConfigurationCreated { get; private set; } = DateTime.UtcNow;

        public void AddDirectory(string directory)
        {
            if (!Directory.Exists(directory)) { return; }
            AddUnique(directory, BaseDirectories, false);
        }

        public void AddOrKey(string orKey, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(orKey)) { return; }
            AddUnique(new SearchKey(orKey, ignoreCase), OrKeys);
        }

        public void AddAndKey(string andKey, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(andKey)) { return; }
            AddUnique(new SearchKey(andKey, ignoreCase), AndKeys);
        }

        public void AddNotKey(string notKey, bool ignoreCase = true)
        {
            if(string.IsNullOrEmpty(notKey)) { return; }
            AddUnique(new SearchKey(notKey, ignoreCase), NotKeys);
        }

        public void AddExtension(string extension, bool ignoreCase = true)
        {
            // TODO Do we have to explicitly put in a period?
            if (string.IsNullOrEmpty(extension)) { return; }
            extension = extension.Contains(".") ? extension : string.Format(".{0}", extension);
            AddUnique(extension, Extensions, ignoreCase);
        }

        private void AddUnique(string value, List<string> collection, bool ignoreCase) 
        {
            if (ignoreCase)
            {
                foreach(var s in collection)
                {
                    // Check to see if value is in collection
                    if (s.IndexOf(value, StringComparison.OrdinalIgnoreCase) >=0) { return; }
                }
                collection.Add(value);
                return;
            }
            else
            {
                foreach (var s in collection)
                {
                    if (s == value) { return; }
                }
                collection.Add(value);
                return;
            }
        }

        public void CreatedBetween(DateTime minimum, DateTime maximum)
        {
            CreatedTime = new DateTimeRange
            {
                MinimumTime = minimum,
                MaximumTime = maximum
            };
        }

        public void LastModifiedBetween(DateTime minimum, DateTime maximum)
        {
            ModifiedTime = new DateTimeRange
            {
                MinimumTime = minimum,
                MaximumTime = maximum
            };
        }

        public void AddCondition(Func<bool> condition)
        {
            Conditions.Add(condition);
        }

        private void AddUnique(SearchKey value, List<SearchKey> collection)
        {
            if (collection.Contains(value)) { return; }
            collection.Add(value);
        }


        /// <summary>
        ///     Serialize Configuration to Json
        /// </summary>
        /// <returns>Json formatted string</returns>
        public string ToJson()
        {
            string result;
            return Serializer.ToJson<SearchConfiguration>(this, out result) ? result : string.Empty;
        }
        /// <summary>
        ///     Create a new SearchConfiguration object from json
        /// </summary>
        /// <param name="json">Json formatted string</param>
        /// <returns>New SearchConfiguration, returns null if failed</returns>
        public static SearchConfiguration FromJson(string json)
        {
            SearchConfiguration result;
            return Serializer.FromJson<SearchConfiguration>(json, out result) ? result : null;
        }
    }
}
