using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
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
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Entry point for DI
            var gateway = new WikipediaGateway(
               @"https://en.wikipedia.org/w/api.php?action=query&list=geosearch&gsradius=10000&gscoord={0}|{1}&gslimit=50&format=json",
               @"https://en.wikipedia.org/w/api.php?action=query&prop=images&pageids={0}&format=json&imlimit=50");

            var processor = SymbolPairsSimilarityProcessor.Instance;

            var window = new MainWindow();

            var vm = new MainViewModel(gateway, processor);
            vm.Latitude = 51.5;
            vm.Longtitude = 0.5;
            window.DataContext = vm;

            window.Show();

            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
