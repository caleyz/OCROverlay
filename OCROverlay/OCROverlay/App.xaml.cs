using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OCROverlay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Console.WriteLine("App ran");
            Setup();            
        }

        public void Setup()
        {
            bool firstRun = OCROverlay.Properties.Settings.Default.FirstRun;
            if (firstRun)
            {
                Console.WriteLine("Was true");
                StartupUri = new Uri("/OCROverlay;component/View/SetupForm.xaml", UriKind.Relative);
                //OCROverlay.Properties.Settings.Default.FirstRun = false;
                //OCROverlay.Properties.Settings.Default.Save();
            }
            else
            {
                Console.WriteLine("Was false");
                StartupUri = new Uri("/OCROverlay;component/View/MainWindow.xaml", UriKind.Relative);
            }
            //OCROverlay.Properties.Settings.Default.FirstRun = true;
            //OCROverlay.Properties.Settings.Default.Save();
        }
    }
}
