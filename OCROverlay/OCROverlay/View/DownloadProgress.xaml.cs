using OCROverlay.Model;
using OCROverlay.ViewModel;
using System;
using System.Collections.Generic;
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

namespace OCROverlay.View
{
    /// <summary>
    /// Interaction logic for DownloadProgress.xaml
    /// </summary>
    public partial class DownloadProgress : Window
    {
        private readonly DownloadProgressVM vm;
        public DownloadProgress(List<LanguageEntry> langEntry)
        {
            InitializeComponent();
            vm = new DownloadProgressVM(langEntry);
            DataContext = vm;
        }
    }
}
