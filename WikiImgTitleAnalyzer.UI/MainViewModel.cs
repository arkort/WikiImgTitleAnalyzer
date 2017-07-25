using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikiImgTitleAnalyzer.Core;
using WikiImgTitleAnalyzer.Interfaces;

namespace WikiImgTitleAnalyzer.UI
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region BL items

        private const int ARTICLE_COUNT = 50;
        private IHttpGateway _gateway;
        private ISimilarityStringProcessor _similarityProcessor;

        #endregion

        #region UI fields, properties and commands
        private double _latitude;
        private double _longtitude;
        private string _similarStrings;

        public double Latitude
        {
            get { return _latitude; }
            set
            {
                _latitude = value;
                OnPropertyChanged();
            }
        }

        public double Longtitude
        {
            get { return _longtitude; }
            set
            {
                _longtitude = value;
                OnPropertyChanged();
            }
        }

        public string SimilarStrings
        {
            get { return _similarStrings; }
            set
            {
                _similarStrings = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartCommand { get; private set; }

        public async void StartExecute()
        {
            await Process();
        }
        #endregion

        public MainViewModel(IHttpGateway gateway, ISimilarityStringProcessor processor)
        {
            _gateway = gateway;
            _similarityProcessor = processor;

            StartCommand = new SimpleCommand(StartExecute);

            Latitude = 70;
            Longtitude = 30;
        }

        public async Task Process()
        {
            SimilarStrings ="Working...";

            var articleIds = await _gateway.GetArticleIdsAsync(Latitude, Longtitude, ARTICLE_COUNT);
            var imageTitles = await _gateway.GetImageTitlesAsync(articleIds.ToArray());

            IEnumerable<string> str = null;

            if (imageTitles.Any())
            {
                var bgTask = new Task(() =>
                {
                    str = _similarityProcessor.GetMostSimilar(imageTitles);
                });
                bgTask.ContinueWith((task) => 
                {
                    SimilarStrings = string.Join("\n", str);
                });
                bgTask.Start();

            }
            else
            {
                SimilarStrings = "No images found.";
            }
        }

        #region NotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
