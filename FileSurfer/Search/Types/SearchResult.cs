using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearcher.Search.Types
{
    public class SearchResult
    {
        public SearchConfiguration Configuration { get; private set; }

        public List<string> SearchResults { get; private set; } = new List<string>();

        public DateTime LastSearched { get; private set; } = DateTime.UtcNow;

        public SearchResult(SearchConfiguration config)
        {
            Configuration = config;
        }

        public void AddMatchingFile(string file)
        {
            if (!SearchResults.Contains(file)) { SearchResults.Add(file); }
        }
    }
}
