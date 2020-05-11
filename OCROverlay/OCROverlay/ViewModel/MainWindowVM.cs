using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCROverlay.ViewModel
{
    public class MainWindowVM : BaseViewModel
    {
        public MainWindowVM()
        {
            ScreenSetup();
        }

        public void ScreenSetup()
        {
            Screen screen = Screen.AllScreens.Where(x => x.DeviceName == Properties.Settings.Default.ChosenScreen).FirstOrDefault();
            Screen = screen;
            Width = screen.Bounds.Width;
            Height = screen.Bounds.Height;
        }

        #region Variables

        private Screen _screen;
        public Screen Screen
        {
            get { return _screen; }
            set
            {
                if (value == _screen)
                    return;
                _screen = value;
                NotifyPropertyChanged("Screen");
            }
        }

        private int _width = 0;
        public int Width
        {
            get { return _width; }
            set
            {
                if (value == _width)
                    return;
                _width = value;
                NotifyPropertyChanged("Width");
            }
        }

        private int _height = 0;
        public int Height
        {
            get { return _height; }
            set
            {
                if (value == _height)
                    return;
                _height = value;
                NotifyPropertyChanged("Height");
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
