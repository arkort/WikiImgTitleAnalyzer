using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WikiImgTitleAnalyzer.Core;
using WikiImgTitleAnalyzer.Core.Gateways;
using WikiImgTitleAnalyzer.UI;

namespace WikiImgTitleAnalyzer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var gateway = new WikipediaGateway(
       @"https://en.wikipedia.org/w/api.php?action=query&list=geosearch&gsradius=10000&gscoord={0}|{1}&gslimit=50&format=json",
       @"https://en.wikipedia.org/w/api.php?action=query&prop=images&pageids={0}&format=json&imlimit=50");

            var processor = JaroWinklerProcessor.Instance;

            var window = new MainWindow();
            window.DataContext = new MainViewModel(gateway, processor);
            window.Show();

            base.OnStartup(e);
        }
    }
}
