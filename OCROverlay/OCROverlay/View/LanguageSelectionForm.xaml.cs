using HtmlAgilityPack;
using OCROverlay.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using OCROverlay.ViewModel;

namespace OCROverlay.View
{
    /// <summary>
    /// Interaction logic for LanguageSelectionFomr.xaml
    /// </summary>
    public partial class LanguageSelectionForm : Window
    {
        private readonly LanguageSelectionVM vm;

        public LanguageSelectionForm()
        {
            InitializeComponent();
            vm = new LanguageSelectionVM();
            DataContext = vm;

            this.Closing += new CancelEventHandler(LanguageSelection_Closing);            
        }

        private TaskCompletionSource<Boolean> _tcs = new TaskCompletionSource<Boolean>();

        public Task<Boolean> Fetch()
        {
            return _tcs.Task;
        }

        void LanguageSelection_Closing(object sender, CancelEventArgs e)
        {
            Console.WriteLine("Language Selection Form Closing");
            //bool res = selectedLanguagesList.Count >= 2 ? true : false;
            bool res = true;
            _tcs.SetResult(res);
        }        
    }
}
