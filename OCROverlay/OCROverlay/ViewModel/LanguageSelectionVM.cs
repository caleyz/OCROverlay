using HtmlAgilityPack;
using OCROverlay.Model;
using OCROverlay.Util;
using OCROverlay.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace OCROverlay.ViewModel
{
    public class LanguageSelectionVM : BaseViewModel
    {
        public LanguageSelectionVM()
        {
            Console.WriteLine("Language Selection VM was initialised");
            GrabLanguagePacks();
        }

        private void GrabLanguagePacks()
        {
            List<LanguageEntry> languageEntries;
            languageEntries = new List<LanguageEntry>();

            var url = "https://tesseract-ocr.github.io/tessdoc/Data-Files";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var tables = doc.DocumentNode.SelectNodes("//table");
            //Could convert this to linq at some point
            foreach (HtmlNode table in tables)
                if (table.SelectNodes("thead/tr/th") != null && table.SelectNodes("thead/tr/th").Count >= 3)
                    if (table.SelectNodes("thead/tr/th")[2].InnerText == "4.0 traineddata")
                        foreach (HtmlNode tableBody in table.SelectNodes("tbody"))
                            foreach (HtmlNode row in tableBody.SelectNodes("tr"))
                            {
                                LanguageEntry currentEntry = new LanguageEntry();
                                currentEntry.ShortCode = row.SelectNodes("th|td")[0].InnerText;
                                currentEntry.LongName = row.SelectNodes("th|td")[1].InnerText;
                                currentEntry.DatapackURL = row.SelectNodes("th|td")[2].Descendants("a").FirstOrDefault().Attributes["href"].Value;
                                languageEntries.Add(currentEntry);
                            }

            AvailableLanguageList = new ObservableCollection<LanguageEntry>(languageEntries.OrderBy(x => x.LongName));
            SelectedLanguageList = new ObservableCollection<LanguageEntry>();
            RetrieveSavedLanguages();
        }

        public void ConfirmLanguages()
        {
            if(SelectedLanguageList.Count >= 2)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, SelectedLanguageList);
                    ms.Position = 0;
                    byte[] buffer = new byte[(int)ms.Length];
                    ms.Read(buffer, 0, buffer.Length);
                    Properties.Settings.Default.SelectedLanguages = Convert.ToBase64String(buffer);
                    Properties.Settings.Default.Save();
                    Close = true;
                }
            }
            else
            {
                string messageBoxText = "You must choose at least 2 languages to use";
                string caption = "Warning";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }

        public void RetrieveSavedLanguages()
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Properties.Settings.Default.SelectedLanguages)))
            {
                if (ms.Length != 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    ObservableCollection<LanguageEntry> holder = (ObservableCollection<LanguageEntry>)bf.Deserialize(ms);
                    if (holder.Count > 0)
                    {
                        foreach (LanguageEntry entry in holder)
                        {
                            SelectedLanguageList.Add(entry);
                            AvailableLanguageList.Remove(AvailableLanguageList.Where(i => i.LongName == entry.LongName).Single());
                        }
                    }
                }
            }
        }

        public void AddSelectedItemsToListView(ObservableCollection<LanguageEntry> languageEntries)
        {
            List<LanguageEntry> languageEntryHolder = languageEntries.ToList();
            foreach (LanguageEntry item in languageEntryHolder)
            {
                SelectedLanguageList.Add(item);
                AvailableLanguageList.Remove(item);
            }
            var sortList = SelectedLanguageList.OrderBy(x => x.LongName);
            SelectedLanguageList = new ObservableCollection<LanguageEntry>(sortList);
        }

        public void RemoveSelectedItemsFromListView(ObservableCollection<LanguageEntry> languageEntries)
        {
            List<LanguageEntry> languageEntryHolder = languageEntries.ToList();
            foreach (LanguageEntry item in languageEntryHolder)
            {
                SelectedLanguageList.Remove(item);
                AvailableLanguageList.Add(item);
            }
            var sortList = AvailableLanguageList.OrderBy(x => x.LongName);
            AvailableLanguageList = new ObservableCollection<LanguageEntry>(sortList);
        }

        public bool ShowWarningMessageBox()
        {
            bool retVal = false;
            string messageBoxText = "Are you sure you wish add all languages? Language packs are very large in size (10mb+ EACH) and adding all language packs will result in downloading ~1GB of data";
            string caption = "Warning";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    retVal = true;
                    break;
                default:
                    break;
            }
            return retVal;
        }

        public void AddAllLanguages()
        {
            if (ShowWarningMessageBox())
            {
                //Could possibly just create a variable to store all available langugages
                List<LanguageEntry> tempAvailList = AvailableLanguageList.ToList();
                List<LanguageEntry> tempSelectList = SelectedLanguageList.ToList();
                List<LanguageEntry> fullList = new List<LanguageEntry>();
                fullList.AddRange(tempAvailList);
                fullList.AddRange(tempSelectList);
                AvailableLanguageList.Clear();
                var sortList = fullList.OrderBy(x => x.LongName);
                SelectedLanguageList = new ObservableCollection<LanguageEntry>(sortList);
            }
        }

        public void RemoveAllLanguages()
        {
            List<LanguageEntry> tempAvailList = AvailableLanguageList.ToList();
            List<LanguageEntry> tempSelectList = SelectedLanguageList.ToList();
            List<LanguageEntry> fullList = new List<LanguageEntry>();
            fullList.AddRange(tempAvailList);
            fullList.AddRange(tempSelectList);            
            SelectedLanguageList.Clear();
            var sortList = fullList.OrderBy(x => x.LongName);
            AvailableLanguageList = new ObservableCollection<LanguageEntry>(sortList);
        }        

        #region Variables

        private bool _close = false;
        public bool Close
        {
            get { return _close; }
            set
            {
                if (value == _close)
                    return;
                _close = value;
                NotifyPropertyChanged("Close");
            }
        }

        private ObservableCollection<LanguageEntry> _availableLanguageListItems = new ObservableCollection<LanguageEntry>();
        public ObservableCollection<LanguageEntry> AvailableLanguageListItems
        {
            get { return _availableLanguageListItems; }
            set
            {
                if (value == _availableLanguageListItems)
                    return;
                _availableLanguageListItems = value;
                NotifyPropertyChanged("AvailableLanguageListItems");
            }
        }

        private ObservableCollection<LanguageEntry> _selectedLanguageListItems = new ObservableCollection<LanguageEntry>();
        public ObservableCollection<LanguageEntry> SelectedLanguageListItems
        {
            get { return _selectedLanguageListItems; }
            set
            {
                if (value == _selectedLanguageListItems)
                    return;
                _selectedLanguageListItems = value;
                NotifyPropertyChanged("SelectedLanguageListItems");
            }
        }

        private ObservableCollection<LanguageEntry> _availableLanguageList = new ObservableCollection<LanguageEntry>();
        public ObservableCollection<LanguageEntry> AvailableLanguageList
        {
            get { return _availableLanguageList; }
            set
            {
                if (value == _availableLanguageList)
                    return;
                _availableLanguageList = value;
                NotifyPropertyChanged("AvailableLanguageList");
            }
        }

        private ObservableCollection<LanguageEntry> _selectedLanguageList = new ObservableCollection<LanguageEntry>();
        public ObservableCollection<LanguageEntry> SelectedLanguageList
        {
            get { return _selectedLanguageList; }
            set
            {
                if (value == _selectedLanguageList)
                    return;
                _selectedLanguageList = value;
                NotifyPropertyChanged("SelectedLanguageList");
            }
        }

        #endregion //Variables

        #region Redirects

        public void AddLanguages_execute(object obj) => AddSelectedItemsToListView(AvailableLanguageListItems);
        public void AddAllLanguages_execute(object obj) => AddAllLanguages();
        public void RemoveLanguages_execute(object obj) => RemoveSelectedItemsFromListView(SelectedLanguageListItems);
        public void RemoveAllLanguages_execute(object obj) => RemoveAllLanguages();
        public void Confirm_execute(object obj) => ConfirmLanguages();

        #endregion //Redirects

        #region CanExecute
        private bool AddLanguages_CanExecute(object obj) => true;
        private bool AddAllLanguages_CanExecute(object obj) => true;
        private bool RemoveLanguages_CanExecute(object obj) => true;
        private bool RemoveAllLanguages_CanExecute(object obj) => true;
        private bool Confirm_CanExecute(object obj) => true;

        #endregion //CanExecute

        #region RelayCommands

        private RelayCommand _addLanguagesCommand;
        public RelayCommand AddLanguagesCommand => _addLanguagesCommand ?? (_addLanguagesCommand = new RelayCommand(AddLanguages_execute, AddLanguages_CanExecute));

        private RelayCommand _addAllLanguagesCommand;
        public RelayCommand AddAllLanguagesCommand => _addAllLanguagesCommand ?? (_addAllLanguagesCommand = new RelayCommand(AddAllLanguages_execute, AddAllLanguages_CanExecute));

        private RelayCommand _removeLanguagesCommand;
        public RelayCommand RemoveLanguagesCommand => _removeLanguagesCommand ?? (_removeLanguagesCommand = new RelayCommand(RemoveLanguages_execute, RemoveLanguages_CanExecute));

        private RelayCommand _removeAllLanguagesCommand;
        public RelayCommand RemoveAllLanguagesCommand => _removeAllLanguagesCommand ?? (_removeAllLanguagesCommand = new RelayCommand(RemoveAllLanguages_execute, RemoveAllLanguages_CanExecute));

        private RelayCommand _confirmCommand;
        public RelayCommand ConfirmCommand => _confirmCommand ?? (_confirmCommand = new RelayCommand(Confirm_execute, Confirm_CanExecute));

        #endregion //RelayCommands
    }
}
