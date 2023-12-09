using FileSearcher.Search;
using FileSearcher.Search.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace ConditionalTest
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

            Func<FileInfo, bool> func = (info) => {
                using (StreamReader sr = new StreamReader(info.FullName))
                {
                    string line = sr.ReadLine();
                    if (line.Contains("right"))
                    {
                        return true;
                    }
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("right"))
                        {
                            return true;
                        }
                    }
                };
                return false;
            };

            SearchConfiguration searchConfig = new SearchConfiguration();

            searchConfig.AddDirectory("C:\\ConditionalSearchTester");
            searchConfig.AddExtension(".txt");
            searchConfig.Recursive = true;
            searchConfig.AddCondition(func);

            SearchTask search = new SearchTask(searchConfig);

            search.SearchCompleted += ((results) =>
            {
                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }
            });

            search.StartSearch();
        }
    }
}
