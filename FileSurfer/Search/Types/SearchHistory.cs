using FileSearcher.Search.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearcher
{
    public static class SearchHistory
    {
        /*private const string Filename = "SearchHistory.json";

        private static readonly StorageFolder _storageFolder = ApplicationData.Current.LocalFolder;

        private static StorageFile _historyFile;

        public static bool Loaded { get; private set; } = false;

        public static List<SearchResult> PreviousSearches { get; private set; }

        public static event Action<bool> HistoryLoaded;

        static SearchHistory()
        {
            LoadHistory();
        }

        public static void LoadHistory()
        {
            Task.Run(() => ReadHistoryFile());
        }

        private static async void ReadHistoryFile()
        {
            _historyFile = await _storageFolder.GetFileAsync(Filename);
            if (_historyFile == null)
            {
                _historyFile = await _storageFolder.CreateFileAsync(Filename);
                Loaded = true;
                HistoryLoaded?.Invoke(false);
                return;
            }

            string json = await FileIO.ReadTextAsync(_historyFile);
            try
            {
                List<SearchResult> history;
                bool success = Serializer.FromJson(json, out history);
                // Order by last searched
                history.OrderBy(x => x.LastSearched);
                PreviousSearches = history;
                HistoryLoaded?.Invoke(success);
                Loaded = true;
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to deserialize search history: " + e.Message);
                Loaded = true;
                HistoryLoaded?.Invoke(false);
            }
        }

        public static void AddSearch(SearchResult result)
        {
            PreviousSearches.Add(result);
            Task.Run(() => WriteSearches());
        }

        private static async void WriteSearches()
        {
            try
            {
                string json;
                bool success = Serializer.ToJson(PreviousSearches, out json);
                if (!success)
                {
                    Logger.LogError("Failed to write to search history");
                    return;
                }
                if (_historyFile == null) {_historyFile = await _storageFolder.GetFileAsync(Filename); }
                else
                {
                    // Clean contents
                    await FileIO.WriteTextAsync(_historyFile, "");
                }
                using (var stream = await _historyFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (var output = stream.GetOutputStreamAt(0))
                    {
                        using (var writer = new Windows.Storage.Streams.DataWriter(output))
                        {
                            writer.WriteString(json);
                            await writer.StoreAsync();
                            await output.FlushAsync();
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Logger.LogError("Failed to write to search history: " + e.Message);
                return;
            }
        }*/

    }
}
