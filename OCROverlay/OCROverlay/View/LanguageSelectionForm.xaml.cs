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

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if(listBox_available_langs.SelectedItems != null)
            {
                var items = listBox_available_langs.SelectedItems.OfType<LanguageEntry>().ToList(); //Can't modify observablecollection while enumerating, so copy
                vm.AddSelectedItemsToListView(items);                
                listBox_available_langs.UnselectAll();
            }
        }

        private void btn_add_all_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Are you sure you wish add all languages? Language packs are very large in size (10mb+ EACH) and adding all language packs will result in downloading ~1GB of data";
            string caption = "Warning";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    vm.AddAllLanguages();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            if(listBox_selected_langs.SelectedItems != null)
            {
                var items = listBox_selected_langs.SelectedItems.OfType<LanguageEntry>().ToList(); //Can't modify observablecollection while enumerating, so copy
                vm.RemoveSelectedItemsFromListView(items);
                listBox_selected_langs.UnselectAll();
            }
        }

        private void btn_remove_all_Click(object sender, RoutedEventArgs e)
        {
            vm.RemoveAllLanguages();
        }

        private void btn_confirm_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("confirm all button was clicked");
            vm.ConfirmLanguages()
        }
    }
}
