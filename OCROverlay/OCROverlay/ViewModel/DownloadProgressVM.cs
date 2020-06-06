using OCROverlay.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OCROverlay.ViewModel
{
    public class DownloadProgressVM : BaseViewModel
    {
        public DownloadProgressVM(List<LanguageEntry> langList)
        {
            PopulateQueue(langList);
            MaxNumber = DownloadQueue.Count;
            DownloadDataPacks();
        }

        public void PopulateQueue(List<LanguageEntry> langList)
        {
            foreach (LanguageEntry entry in langList)
                DownloadQueue.Enqueue(entry);
        }

        async void DownloadDataPacks()
        {
            MaxNumber = DownloadQueue.Count;
            while(DownloadQueue.Count > 0)
            {             
                LanguageEntry entry = DownloadQueue.Dequeue();
                LanguageDownloading = entry.LongName;
                await DownloadDP(new Uri(entry.DatapackURL));
                IncrementItemNumber();
            }
            DownloadCompleteVisibility = Visibility.Visible;
        }

        public async Task DownloadDP(Uri uri)
        {
            WebClient wc = new WebClient();
            wc.DownloadProgressChanged += wc_DownloadProgressChanged;
            await wc.DownloadFileTaskAsync(uri, Path.Combine(Properties.Settings.Default.DownloadLocation, uri.ToString().Substring(uri.ToString().LastIndexOf('/') + 1)));
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            CurrentPercentage = e.ProgressPercentage;
        }

        public void CalculateProgressPercentage()
        {
            CompletedPercentage = Math.Round(Convert.ToDouble((100 / MaxNumber) * CurrentNumber), 2);
        }

        public void IncrementItemNumber()
        {
            CurrentNumber++;
            UpdateItemText();
            CalculateProgressPercentage();
        }

        public void UpdateItemText()
        {
            ItemNumberText = String.Format("{0}/{1}", CurrentNumber, MaxNumber);
        }

        #region Variables

        private Visibility _downloadCompleteVisibility = Visibility.Hidden;
        public Visibility DownloadCompleteVisibility
        {
            get { return _downloadCompleteVisibility; }
            set
            {
                if (value == _downloadCompleteVisibility)
                    return;
                _downloadCompleteVisibility = value;
                NotifyPropertyChanged("DownloadCompleteVisibility");
            }
        }

        private Queue<LanguageEntry> _downloadQueue = new Queue<LanguageEntry>();
        public Queue<LanguageEntry> DownloadQueue
        {
            get { return _downloadQueue; }
            set
            {
                if (value == _downloadQueue)
                    return;
                _downloadQueue = value;
                NotifyPropertyChanged("DownloadQueue");
            }
        }

        private double _currentPercentage = 0;
        public double CurrentPercentage
        {
            get { return _currentPercentage; }
            set
            {
                if (value == _currentPercentage)
                    return;
                _currentPercentage = value;
                NotifyPropertyChanged("CurrentPercentage");
            }
        }

        private double _completedPercentage = 0;
        public double CompletedPercentage
        {
            get { return _completedPercentage; }
            set
            {
                if (value == _completedPercentage)
                    return;
                _completedPercentage = value;
                NotifyPropertyChanged("CompletedPercentage");
            }
        }

        private int _currentNumber = 0;
        public int CurrentNumber
        {
            get { return _currentNumber; }
            set
            {
                if (value == _currentNumber)
                    return;
                _currentNumber = value;
                NotifyPropertyChanged("CurrentNumber");
            }
        }

        private int _maxNumber = 0;
        public int MaxNumber
        {
            get { return _maxNumber; }
            set
            {
                if (value == _maxNumber)
                    return;
                _maxNumber = value;
                NotifyPropertyChanged("MaxNumber");
            }
        }

        private string _languageDownloading = "";
        public string LanguageDownloading
        {
            get { return _languageDownloading; }
            set
            {
                if (value == _languageDownloading)
                    return;
                _languageDownloading = value + " Datapack";
                NotifyPropertyChanged("LanguageDownloading");
            }
        }

        private string _itemNumberText = "";
        public string ItemNumberText
        {
            get { return _itemNumberText; }
            set
            {
                if (value == _itemNumberText)
                    return;
                _itemNumberText = value;
                NotifyPropertyChanged("ItemNumberText");
            }
        }

        #endregion //Variables

        #region Redirects

        #endregion //Redirects

        #region CanExecute

        #endregion //CanExecute

        #region RelayCommands

        #endregion //RelayCommands
    }
}
