using FileSearcher.Search;
using FileSearcher.Search.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileSurfer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SearchConfiguration searchConfig = new SearchConfiguration();

            searchConfig.AddAndKey("boost");
            searchConfig.AddDirectory("C:\\");
            searchConfig.AddDirectory("I:\\");
            searchConfig.AddDirectory("F:\\");
            searchConfig.AddDirectory("E:\\");
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
