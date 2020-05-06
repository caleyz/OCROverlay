using OCROverlay.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCROverlay.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public delegate void ViewModelChangeEventHandler(object sender, ViewModelChangeEventArgs e);
        public event ViewModelChangeEventHandler ViewModelChangeRequest;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void ChangeViewModel(string viewModelName, object data)
        {
            ViewModelChangeEventHandler eventHandler = ViewModelChangeRequest;
            if(eventHandler != null)
            {
                eventHandler(this, new ViewModelChangeEventArgs(viewModelName, data));
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler eventHandler = PropertyChanged;
            if(eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
