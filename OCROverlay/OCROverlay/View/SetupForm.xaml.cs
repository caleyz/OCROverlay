﻿using OCROverlay.ViewModel;
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

namespace OCROverlay.View
{
    /// <summary>
    /// Interaction logic for FirstRun.xaml
    /// </summary>
    public partial class SetupForm : Window
    {
        private readonly SetupVM vm;
        public SetupForm()
        {            
            InitializeComponent();
            vm = new SetupVM();
            DataContext = vm;
            this.Closing += new CancelEventHandler(SetupForm_Closing);
            ImageSetup();
            ScreenCheck();
        }

        void SetupForm_Closing(object sender, CancelEventArgs e)
        {
            //closing code
        }

        private void ImageSetup()
        {
            //for some reason setting image sources in XAML to the project Resources defaults it to the path C:\WINDOWS\system32... - Have to bypass
            img_lang_cross.Source = img_screen_cross.Source =
                new BitmapImage(new Uri("/OCROverlay;component/Resources/cross.png", UriKind.Relative));
            img_lang_tick.Source = img_screen_tick.Source =
                new BitmapImage(new Uri("/OCROverlay;component/Resources/tick.png", UriKind.Relative));
        }

        private void ScreenCheck()
        {
            vm.ScreenCheck();            
        }

        private void SetupLanguages()
        {            
            vm.InitialiseLanguageForm();
        }

        private void ResetLanguageImages()
        {
            img_lang_tick.Visibility = Visibility.Hidden;
            img_lang_cross.Visibility = Visibility.Hidden;
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