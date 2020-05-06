using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCROverlay.Events
{
    public class ViewModelChangeEventArgs : EventArgs
    {
        public virtual string ViewModelName { get; private set; }
        public virtual object Data { get; private set; }
        public ViewModelChangeEventArgs(string viewModelName)
        {
            ViewModelName = viewModelName;
        }

        public ViewModelChangeEventArgs(string viewModelName, object data)
        {
            ViewModelName = viewModelName;
            Data = data;
        }
    }
}
