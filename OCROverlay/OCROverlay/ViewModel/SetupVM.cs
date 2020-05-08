using Microsoft.WindowsAPICodePack.Dialogs;
using OCROverlay.Util;
using OCROverlay.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace OCROverlay.ViewModel
{
    public class SetupVM : BaseViewModel
    {
        public SetupVM()
        {
            Properties.Settings.Default.Reset();
            ScreenCheck();
            DownloadLocationSetup();            
            ///////////            
        }

        public void ResetLanguageImages()
        {
            LanguageCrossVisibility = LanguageTickVisibility = Visibility.Hidden;
        }

        public void ResetScreenImages()
        {
            ScreenCrossVisibility = ScreenTickVisibility = Visibility.Hidden;
        }

        public void DownloadLocationSetup()
        {
            string finalPath;
            if (String.IsNullOrEmpty(Properties.Settings.Default.DownloadLocation))
            {
                string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string myFolder = "OCROverlay\\DataPacks";
                finalPath = Path.Combine(basePath, myFolder);
                try
                {
                    System.IO.Directory.CreateDirectory(finalPath);
                }
                catch(Exception e)
                {
                    throw e;
                }
                Properties.Settings.Default.DownloadLocation = finalPath;
                Properties.Settings.Default.Save();
            }
            else
            {
                finalPath = Properties.Settings.Default.DownloadLocation;
            }
            DownloadLocationText = finalPath;
        }

        public void ConfirmSettings()
        {
            //
        }

        public void ChooseScreen()
        {
            //
        }

        public void ScreenCheck()
        {
            if (Screen.AllScreens.Length > 1)
                ScreenButtonEnabled = true;
            else
                ScreenTickVisibility = Visibility.Visible;
        }

        public async void InitialiseLanguageForm()
        {
            ResetLanguageImages();
            LanguageSelectionForm langForm = new LanguageSelectionForm();
            langForm.ShowDialog();
            bool value = await langForm.Fetch();
        }

        public void ChooseDownloadLocation()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = Path.GetFullPath(Properties.Settings.Default.DownloadLocation);
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Console.WriteLine("You selected: " + dialog.FileName);
            }
        }        

        #region Variables

        private string _downloadLocationText = "";
        public string DownloadLocationText
        {
            get { return _downloadLocationText; }
            set
            {
                if (value == _downloadLocationText)
                    return;
                _downloadLocationText = value;
                NotifyPropertyChanged("DownloadLocationText");
            }
        }

        private Boolean _screenButtonEnabled = false;
        public Boolean ScreenButtonEnabled
        {
            get { return _screenButtonEnabled; }
            set
            {
                if (value == _screenButtonEnabled)
                    return;
                _screenButtonEnabled = value;
                NotifyPropertyChanged("ScreenButtonEnabled");
            }
        }

        private Visibility _languageTickVisibility = Visibility.Hidden;
        public Visibility LanguageTickVisibility
        {
            get { return _languageTickVisibility; }
            set
            {
                if (value == _languageTickVisibility)
                    return;
                _languageTickVisibility = value;
                NotifyPropertyChanged("LanguageTickVisibility");
            }
        }

        private Visibility _languageCrossVisibility = Visibility.Hidden;
        public Visibility LanguageCrossVisibility
        {
            get { return _languageCrossVisibility; }
            set
            {
                if (value == _languageCrossVisibility)
                    return;
                _languageCrossVisibility = value;
                NotifyPropertyChanged("LanguageCrossVisibility");
            }
        }

        private Visibility _screenTickVisibility = Visibility.Hidden;
        public Visibility ScreenTickVisibility
        {
            get { return _screenTickVisibility; }
            set
            {
                if (value == _screenTickVisibility)
                    return;
                _screenTickVisibility = value;
                NotifyPropertyChanged("ScreenTickVisibility");
            }
        }

        private Visibility _screenCrossVisibility = Visibility.Hidden;
        public Visibility ScreenCrossVisibility
        {
            get { return _screenCrossVisibility; }
            set
            {
                if (value == _screenCrossVisibility)
                    return;
                _screenCrossVisibility = value;
                NotifyPropertyChanged("ScreenCrossVisibility");
            }
        }

        #endregion //Variables

        #region Redirects

        public void DownloadLocation_execute(object obj) => ChooseDownloadLocation();
        public void SetupLanguages_execute(object obj) => InitialiseLanguageForm();
        public void ChooseScreen_execute(object obj) => ChooseScreen();
        public void Confirm_execute(object obj) => ConfirmSettings();

        #endregion //Redirects

        #region CanExecute
        private bool DownloadLocation_CanExecute(object obj) => true;
        private bool SetupLanguages_CanExecute(object obj) => true;
        private bool ChooseScreen_CanExecute(object obj) => ScreenButtonEnabled;
        private bool Confirm_CanExecute(object obj) => true;

        #endregion //CanExecute

        #region RelayCommands

        private RelayCommand _downloadLocationCommand;
        public RelayCommand DownloadLocationCommand => _downloadLocationCommand ?? (_downloadLocationCommand = new RelayCommand(DownloadLocation_execute, DownloadLocation_CanExecute));

        private RelayCommand _setupLanguagesCommand;
        public RelayCommand SetupLanguagesCommand => _setupLanguagesCommand ?? (_setupLanguagesCommand = new RelayCommand(SetupLanguages_execute, SetupLanguages_CanExecute));

        private RelayCommand _chooseScreenCommand;
        public RelayCommand ChooseScreenCommand => _chooseScreenCommand ?? (_chooseScreenCommand = new RelayCommand(ChooseScreen_execute, ChooseScreen_CanExecute));

        private RelayCommand _confirmCommand;
        public RelayCommand ConfirmCommand => _confirmCommand ?? (_confirmCommand = new RelayCommand(Confirm_execute, Confirm_CanExecute));

        #endregion //RelayCommands
    }
}
