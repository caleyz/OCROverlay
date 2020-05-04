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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OCROverlay
{
    /// <summary>
    /// Interaction logic for FirstRun.xaml
    /// </summary>
    public partial class FirstRunForm : Window
    {
        public FirstRunForm()
        {            
            InitializeComponent();
            this.Closing += new CancelEventHandler(FirstRun_Closing);
            RunChecks();
        }

        void FirstRun_Closing(object sender, CancelEventArgs e)
        {
            //closing code
        }

        private void RunChecks()
        {
            if (Screen.AllScreens.Length > 1)
                btn_screens.IsEnabled = true;
            else
                img_screen_tick.Visibility = Visibility.Visible;
        }

        private async void SetupLanguages()
        {
            ResetLanguageImages();
            LanguageSelectionForm langForm = new LanguageSelectionForm();
            langForm.ShowDialog();
            bool value = await langForm.Fetch();
            //if (value)
            //    img_lang_tick.Visibility = Visibility.Visible;
            //else
            //    img_lang_cross.Visibility = Visibility.Visible;
            //((FirstRunForm)System.Windows.Application.Current.FirstRunForm.UpdateLayout();
        }

        private void ResetLanguageImages()
        {
            img_lang_tick.Visibility = Visibility.Hidden;
            img_lang_cross.Visibility = Visibility.Hidden;
        }

        private void ResetScreenImages()
        {
            img_screen_tick.Visibility = Visibility.Hidden;
            img_screen_cross.Visibility = Visibility.Hidden;
        }

        private void btn_languages_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("languages button was clicked");
            SetupLanguages();
        }

        private void btn_screens_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("screens button was clicked");
        }

        private void btn_confirm_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("confirm button was clicked");
        }
    }
}
