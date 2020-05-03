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

namespace OCROverlay
{
    /// <summary>
    /// Interaction logic for LanguageSelectionFomr.xaml
    /// </summary>
    public partial class LanguageSelectionForm : Window
    {
        public LanguageSelectionForm()
        {
            InitializeComponent();
            this.Closing += new CancelEventHandler(LanguageSelection_Closing);
        }

        void LanguageSelection_Closing(object sender, CancelEventArgs e)
        {
            //closing code
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("add button was clicked");
        }

        private void btn_add_all_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("add all button was clicked");
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("remove button was clicked");
        }

        private void btn_remove_all_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("remove all button was clicked");
        }
    }
}
