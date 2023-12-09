using FileSearcher.Search.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileSearcher.Search
{
    public class SearchTask : INotifyPropertyChanged
    {
        public SearchConfiguration Configuration { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public event Action<List<string>> SearchCompleted;

        public List<string> MatchingFiles;

        private ConcurrentQueue<string> _files;

        private SearchResult _searchResult;

        private volatile bool _running = false;

        public static SearchTask BuildFromPrevious(SearchResult result)
        {
            SearchTask newSearch = new SearchTask();
            newSearch.Configuration = result.Configuration;
            return newSearch;
        }

        public SearchTask()
        {
            Configuration = new SearchConfiguration();
        }

        public SearchTask(SearchConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void StartSearch()
        { 
            _files = new ConcurrentQueue<string>();
            MatchingFiles = new List<string>();
            List<string> directories = Configuration.BaseDirectories;
            _running = true;
            Task.Run(() => SearchBaseDirectories(directories));
            Task.Run(() => CheckFiles());
        }

        public void StopSearch()
        {
            _running = false;
        }

        private void SearchBaseDirectories(List<string> directories)
        {
            foreach(var dir in directories)
            {
                SearchDirectory(dir);
                if (!_running) { return; }
            }
            _running = false;
        }

        private void SearchDirectory(string directory)
        {
            if (!_running) { return; }
            Console.WriteLine("Searching " + directory);
            // Get all files in this direction
            var files = Directory.GetFiles(directory);
            for(int i = 0; i < files.Length; ++i)
            {
                _files.Enqueue(files[i]);
            }
            if (Configuration.Recursive)
            {
                var directories = Directory.GetDirectories(directory);
                for (int i = 0; i < directories.Length; ++i)
                {
                    SearchDirectory(directories[i]);
                }
            }
        }

        private void CheckFiles()
        {
            List<SearchKey> notKeys = Configuration.NotKeys;
            List<SearchKey> andKeys = Configuration.AndKeys;
            List<SearchKey> orKeys = Configuration.OrKeys;
            List<string> extensions = Configuration.Extensions;

            while (_running || _files.Count() > 0)
            {
                string file;
                if (_files.TryDequeue(out file))
                {
                    bool success = false;
                    // Check extensions
                    foreach (var extension in extensions)
                    {
                        if (Path.GetExtension(file).
                            IndexOf(extension, StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            continue;
                        }
                    }
                    // Check nots
                    foreach (var notKey in notKeys)
                    {
                        if (!CheckKey(file, notKey, false)) { continue; }
                    }
                    // Check ands
                    foreach(var andKey in andKeys)
                    {
                        if (!CheckKey(file, andKey, true)) { continue; }
                    }
                    // Check ors
                    foreach (var orKey in orKeys)
                    {
                        success |= CheckKey(file, orKey, true);
                    }
                    if (success)
                    {
                        Console.WriteLine("Found file " + file);
                        MatchingFiles.Add(file);
                    }
                }
                Thread.Sleep(1);
            }
            SearchCompleted?.Invoke(MatchingFiles);
        }

        private bool CheckKey(string filename, SearchKey key, bool expectedValue)
        {
            StringComparison comparisonType = key.IgnoreCase ? 
                StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture;
            return expectedValue == filename.IndexOf(key.Key, comparisonType) >= 0;
        }
    }
}
