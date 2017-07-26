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
        private const string SIMILARITY_PERCENTAGE_MESSAGE = "These titles are {0}-{1}% similar";
        private const string LOWER_SIMILARITY_PERCENTAGE_MESSAGE = "There are {0} more titles that are less than {1}% similar";

        private double _latitude;
        private double _longtitude;
        private string _similarStrings;
        private string _similarityPercentageString;
        private string _lowerSimilarityPercentageString;

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

        public string SimilarityPercentageString
        {
            get { return _similarityPercentageString; }
            set
            {
                _similarityPercentageString = value;
                OnPropertyChanged();
            }
        }

        public string LowerSimilarityPercentageString
        {
            get { return _lowerSimilarityPercentageString; }
            set
            {
                _lowerSimilarityPercentageString = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartCommand { get; private set; }

        public async Task StartExecute()
        {
            await ProcessGettingSimilarStrings();
        }

        private void SetSimilarityPercentageString(int similarityIndex)
        {
            SimilarityPercentageString = string.Format(SIMILARITY_PERCENTAGE_MESSAGE, similarityIndex * 10, (similarityIndex + 1) * 10);
        }

        private void SetLowerSimilarityPercentageString(int similarityIndex, int count)
        {
            LowerSimilarityPercentageString = string.Format(LOWER_SIMILARITY_PERCENTAGE_MESSAGE, count, similarityIndex * 10);
        }

        #endregion

        public MainViewModel(IHttpGateway gateway, ISimilarityStringProcessor processor)
        {
            _gateway = gateway;
            _similarityProcessor = processor;

            StartCommand = new SimpleCommand(async () => await StartExecute());
        }

        public async Task ProcessGettingSimilarStrings()
        {
            SimilarStrings = "Working...";
            SimilarityPercentageString = string.Empty;
            LowerSimilarityPercentageString = string.Empty;

            var articleIds = await _gateway.GetArticleIdsAsync(Latitude, Longtitude, ARTICLE_COUNT);
            var imageTitles = await _gateway.GetImageTitlesAsync(articleIds.ToArray());

            ISimilarityResult similarityResult = null;

            if (imageTitles.Any())
            {
                var bgTask = new Task(() =>
                {
                    similarityResult = _similarityProcessor.GetMostSimilar(imageTitles);
                });
                bgTask.Start();
                await bgTask;

                SimilarStrings = string.Join("\n", similarityResult.SimilarStrings);
                SetSimilarityPercentageString(similarityResult.SimilarityIndex);
                SetLowerSimilarityPercentageString(similarityResult.SimilarityIndex, imageTitles.Count() - similarityResult.SimilarStrings.Count);
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
