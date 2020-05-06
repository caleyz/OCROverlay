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

            //foreach(LanguageEntry entry in languageEntries)
            //{
            //    Console.WriteLine("{0}:{1}:{2}", entry.shortCode, entry.longName, entry.datapackURL);
            //}

            AvailableLanguageList = new ObservableCollection<LanguageEntry>(languageEntries);
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

        public void AddSelectedItemsToListView(List<LanguageEntry> languageEntries)
        {
            foreach (LanguageEntry item in languageEntries)
            {
                SelectedLanguageList.Add(item);
                AvailableLanguageList.Remove(item);
            }
            SelectedLanguageList.OrderBy(n => n.LongName);
        }

        public void RemoveSelectedItemsFromListView(List<LanguageEntry> languageEntries)
        {
            foreach (LanguageEntry item in languageEntries)
            {
                SelectedLanguageList.Remove(item);
                AvailableLanguageList.Add(item);
            }
            AvailableLanguageList.OrderBy(n => n.LongName);
        }

        public void AddAllLanguages()
        {
            //Could possibly just create a variable to store all available langugages
            List<LanguageEntry> tempAvailList = AvailableLanguageList.ToList();
            List<LanguageEntry> tempSelectList = SelectedLanguageList.ToList();
            List<LanguageEntry> fullList = new List<LanguageEntry>();
            fullList.AddRange(tempAvailList);
            fullList.AddRange(tempSelectList);
            AvailableLanguageList.Clear();
            SelectedLanguageList = new ObservableCollection<LanguageEntry>(fullList);
        }

        public void RemoveAllLanguages()
        {
            List<LanguageEntry> tempAvailList = AvailableLanguageList.ToList();
            List<LanguageEntry> tempSelectList = SelectedLanguageList.ToList();
            List<LanguageEntry> fullList = new List<LanguageEntry>();
            fullList.AddRange(tempAvailList);
            fullList.AddRange(tempSelectList);
            SelectedLanguageList.Clear();
            AvailableLanguageList = new ObservableCollection<LanguageEntry>(fullList);
            AvailableLanguageList.OrderBy(n => n.LongName);
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

        #region RelayCommands

        //public RelayCommand<ICloseable> CloseWindowCommand { get; private set; }

        #endregion //RelayCommands
    }
}
