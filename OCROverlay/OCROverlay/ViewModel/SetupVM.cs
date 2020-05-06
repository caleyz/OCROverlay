using OCROverlay.View;
using System;
using System.Collections.Generic;
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
            //do something
        }

        public void ResetLanguageImages()
        {
            LanguageCrossVisibility = LanguageTickVisibility = Visibility.Hidden;
        }

        public void ResetScreenImages()
        {
            ScreenCrossVisibility = ScreenTickVisibility = Visibility.Hidden;
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

        #region Variables

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
    }
}
