using FileSearcher.Search;
using FileSearcher.Search.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FileSearcher
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SearchConfiguration searchConfig = new SearchConfiguration();

            searchConfig.AddAndKey("boost");
            searchConfig.AddDirectory("C:\\");
            searchConfig.AddNotKey("old");
            searchConfig.AddOrKey("feature");
            searchConfig.AddExtension("cpp");
            searchConfig.Recursive = true;

            Stopwatch watch = null;

            SearchTask search = new SearchTask(searchConfig);

            search.SearchCompleted += ((results) =>
            {
                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }
                Console.WriteLine("Search took: " + (watch.ElapsedMilliseconds / 1000).ToString() + " seconds");
            });

            search.StartSearch();
            watch = Stopwatch.StartNew();
        }
    }
}
