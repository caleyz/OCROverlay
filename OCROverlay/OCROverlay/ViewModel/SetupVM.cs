using Microsoft.WindowsAPICodePack.Dialogs;
using OCROverlay.Model;
using OCROverlay.Util;
using OCROverlay.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

namespace OCROverlay.ViewModel
{
    public class SetupVM : BaseViewModel
    {
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        public SetupVM()
        {            
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

        public void ChooseHotkey()
        {
            bool isPressed = false;
            KeysPressedText = "Press Keys";
            while (!isPressed)
            {
                
            }
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

        public void SaveSelectedScreen()
        {
            Screen chosenScreen = Screen.AllScreens.Where(x => x.DeviceName.Substring(4) == ScreenListSelected.DeviceName).FirstOrDefault();
            Properties.Settings.Default.ChosenScreen = chosenScreen.DeviceName;
        }

        public void ConfirmSettings()
        {
            //Should probably clean this up
            if(ScreenListSelected != null)
                SaveSelectedScreen();
            else
            {
                return;
            }
            if (!String.IsNullOrEmpty(Properties.Settings.Default.SelectedLanguages))
            {
                //
            }
            else
            {
                return;
            }
            Properties.Settings.Default.FirstRun = false;
            Properties.Settings.Default.Save();            
            RestartApp(Process.GetCurrentProcess().Id, Process.GetCurrentProcess().ProcessName);
            //System.Windows.Forms.Application.Restart();
            ChangeViewModel("MainWindowVM", new object());
        }

        static void RestartApp(int pid, string applicationName)
        {
            // Wait for the process to terminate
            Process process = null;
            try
            {
                process = Process.GetProcessById(pid);
                process.WaitForExit(1000);
            }
            catch (ArgumentException ex)
            {
                // ArgumentException to indicate that the process doesn't exist
            }
            Process.Start(applicationName, "");
        }

        public void ScreenCheck()
        {
            ObservableCollection<Screen> screenHolder = new ObservableCollection<Screen>(Screen.AllScreens);
            foreach (Screen screen in screenHolder)
            {
                ScreenEntry newScreen = new ScreenEntry();
                newScreen.DeviceName = screen.DeviceName.Substring(4);
                newScreen.Height = screen.Bounds.Height;
                newScreen.Width = screen.Bounds.Width;
                newScreen.CombinedValue = String.Format("{0} ({1}x{2})", newScreen.DeviceName, newScreen.Width, newScreen.Height);
                ScreenList.Add(newScreen);
            }
            ScreenListSelected = ScreenList[0];
        }

        public async void InitialiseLanguageForm()
        {
            ResetLanguageImages();
            LanguageSelectionForm langForm = new LanguageSelectionForm();
            langForm.ShowDialog();
            bool value = await langForm.Fetch();
            Console.WriteLine(value);
            if (value)
            {
                //dispatcher.Invoke(new Action(() => { LanguageTickVisibility = Visibility.Visible; }), DispatcherPriority.Normal);
                LanguageTickVisibility = Visibility.Visible;
            }
            else
            {
                //dispatcher.Invoke(new Action(() => { LanguageCrossVisibility = Visibility.Visible; }), DispatcherPriority.Normal);
                LanguageCrossVisibility = Visibility.Visible;
            }
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

        public void KeyDownEventStart(object obj)
        {
            System.Windows.Input.KeyEventArgs args = (System.Windows.Input.KeyEventArgs)obj;
            Console.WriteLine("Hello World Down");
            
        }

        public void KeyUpEventStart(object obj)
        {
            System.Windows.Input.KeyEventArgs args = (System.Windows.Input.KeyEventArgs)obj;
            Console.WriteLine("Hello World Up");

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

        private ObservableCollection<ScreenEntry> _screenList = new ObservableCollection<ScreenEntry>();
        public ObservableCollection<ScreenEntry> ScreenList
        {
            get { return _screenList; }
            set
            {
                if (value == _screenList)
                    return;
                _screenList = value;
                NotifyPropertyChanged("ScreenList");
            }
        }

        private ScreenEntry _screenListSelected = new ScreenEntry();
        public ScreenEntry ScreenListSelected
        {
            get { return _screenListSelected; }
            set
            {
                if (value == _screenListSelected)
                    return;
                _screenListSelected = value;
                NotifyPropertyChanged("ScreenListSelected");
            }
        }

        private string _keysPressedText = "";
        public string KeysPressedText
        {
            get { return _keysPressedText; }
            set
            {
                if (value == _keysPressedText)
                    return;
                _keysPressedText = value;
                NotifyPropertyChanged("KeysPressedText");
            }
        }

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
        public void ChooseHotkey_execute(object obj) => ChooseHotkey();
        public void Confirm_execute(object obj) => ConfirmSettings();
        public void KeyDownEvent_execute(object obj) => KeyDownEventStart(obj);
        public void KeyUpEvent_execute(object obj) => KeyUpEventStart(obj);

        #endregion //Redirects

        #region CanExecute
        private bool DownloadLocation_CanExecute(object obj) => true;
        private bool SetupLanguages_CanExecute(object obj) => true;
        private bool ChooseHotkey_CanExecute(object obj) => true;
        private bool Confirm_CanExecute(object obj) => true;
        private bool KeyDownEvent_CanExecute(object obj) => true;
        private bool KeyUpEvent_CanExecute(object obj) => true;

        #endregion //CanExecute

        #region RelayCommands

        private RelayCommand _downloadLocationCommand;
        public RelayCommand DownloadLocationCommand => _downloadLocationCommand ?? (_downloadLocationCommand = new RelayCommand(DownloadLocation_execute, DownloadLocation_CanExecute));

        private RelayCommand _setupLanguagesCommand;
        public RelayCommand SetupLanguagesCommand => _setupLanguagesCommand ?? (_setupLanguagesCommand = new RelayCommand(SetupLanguages_execute, SetupLanguages_CanExecute));

        private RelayCommand _chooseHotkeyCommand;
        public RelayCommand ChooseHotkeyCommand => _chooseHotkeyCommand ?? (_chooseHotkeyCommand = new RelayCommand(ChooseHotkey_execute, ChooseHotkey_CanExecute));

        private RelayCommand _confirmCommand;
        public RelayCommand ConfirmCommand => _confirmCommand ?? (_confirmCommand = new RelayCommand(Confirm_execute, Confirm_CanExecute));

        private RelayCommand _keyDownEvent;
        public RelayCommand KeyDownEvent => _keyDownEvent ?? (_keyDownEvent = new RelayCommand(KeyDownEvent_execute, KeyDownEvent_CanExecute));

        private RelayCommand _keyUpEvent;
        public RelayCommand KeyUpEvent => _keyUpEvent ?? (_keyUpEvent = new RelayCommand(KeyUpEvent_execute, KeyUpEvent_CanExecute));

        #endregion //RelayCommands
    }
}
