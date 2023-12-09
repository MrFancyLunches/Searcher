using FileSearcher.Search.Types;
using FileSurfer.Search.Types;
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
            List<Task> tasks = new List<Task>();
            Parallel.ForEach(directories, dir =>
            {
                SearchDirectory(dir, dir.Contains("C:\\") || dir.Contains("I:\\"));
            });
            _running = false;
        }

        private void SearchDirectory(string directory, bool threaded)
        {
            if (!_running) { return; }
            // Get all files in this direction
            string[] files = null;
            try
            {
                files = Directory.GetFiles(directory);
            }
            catch(Exception e)
            {
                return;
            }
            for(int i = 0; i < files.Length; ++i)
            {
                _files.Enqueue(files[i]);
            }
            if (Configuration.Recursive)
            {
                try
                {
                    var directories = Directory.GetDirectories(directory);
                    if (!threaded)
                    {
                        foreach (var d in directories)
                        {
                            SearchDirectory(d, threaded);
                        }
                    }
                    else
                    {
                        Parallel.ForEach(directories, d =>
                        {
                            SearchDirectory(d, threaded);
                        });
                    }
                }
                catch(Exception e) { return; }
            }
        }

        private void CheckFiles()
        {
            List<SearchKey> notKeys = Configuration.NotKeys;
            List<SearchKey> andKeys = Configuration.AndKeys;
            List<SearchKey> orKeys = Configuration.OrKeys;
            List<string> extensions = Configuration.Extensions;

            DateTimeRange modifiedTime = Configuration.ModifiedTime;
            DateTimeRange createdTime = Configuration.CreatedTime;
            bool checkModified = modifiedTime != null;
            bool checkCreatedTime = createdTime != null;

            while (_running || _files.Count() > 0)
            {
                string fullName;
                if (_files.TryDequeue(out fullName))
                {
                    string file = Path.GetFileName(fullName);
                    bool success = false;
                    bool validExtension = true;

                    // Check if we need to pull file info
                    if (checkModified || checkCreatedTime)
                    {
                        FileInfo info = new FileInfo(fullName);
                        if (checkModified)
                        {
                            if (!info.LastAccessTime.InRange(modifiedTime))
                            {
                                continue;
                            }
                        }

                        if (checkCreatedTime)
                        {
                            if (!info.CreationTime.InRange(createdTime))
                            {
                                continue;
                            }
                        }
                    }

                    // Check extensions
                    foreach (var extension in extensions)
                    {
                        if (Path.GetExtension(file).IndexOf
                            (extension, StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            validExtension = false;
                            break;
                        }
                    }
                    if (!validExtension) { continue; }
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
                        MatchingFiles.Add(fullName);
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
