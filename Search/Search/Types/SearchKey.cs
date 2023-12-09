using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearcher.Search.Types
{
    public class SearchKey
    {
        public bool IgnoreCase { get; private set; }

        public string Key { get; private set; }

        private SearchKey() { }

        public SearchKey(string key, bool ignoreCase = true)
        {
            Key = key;
            IgnoreCase = ignoreCase;
        }

        public static bool operator ==(SearchKey lhs, SearchKey rhs)
        {
            if (lhs.IgnoreCase != rhs.IgnoreCase) { return false; }
            StringComparison comparison = lhs.IgnoreCase ? 
                StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture;
            return string.Equals(lhs.Key, rhs.Key, comparison);
        }

        public static bool operator !=(SearchKey lhs, SearchKey rhs)
        {
            if (lhs.IgnoreCase != rhs.IgnoreCase) { return true; }
            StringComparison comparison = lhs.IgnoreCase ?
                StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture;
            return !string.Equals(lhs.Key, rhs.Key, comparison);
        }

        public override bool Equals(object obj)
        {
            if (obj is SearchKey)
            {
                SearchKey other = obj as SearchKey;
                if (other.IgnoreCase != IgnoreCase) { return false; }
                StringComparison comparison = IgnoreCase ?
                    StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture;
                bool equals = Key.Equals(other.Key, comparison);
                return equals;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = 2122842883;
            hashCode = hashCode * -1521134295 + IgnoreCase.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Key);
            return hashCode;
        }
    }
}
