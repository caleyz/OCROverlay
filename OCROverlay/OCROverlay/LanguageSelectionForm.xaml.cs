using HtmlAgilityPack;
using OCROverlay.Models;
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
        List<LanguageEntry> languageEntries;
        List<LanguageEntry> availableLanguagesList;
        List<LanguageEntry> selectedLanguagesList;

        public LanguageSelectionForm()
        {
            InitializeComponent();
            this.Closing += new CancelEventHandler(LanguageSelection_Closing);
            GrabLanguagePacks();
        }

        void LanguageSelection_Closing(object sender, CancelEventArgs e)
        {
            //closing code
        }

        private void GrabLanguagePacks()
        {
            languageEntries = new List<LanguageEntry>();

            var url = "https://tesseract-ocr.github.io/tessdoc/Data-Files";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var tables = doc.DocumentNode.SelectNodes("//table");
            //Could convert this to linq at some point
            foreach (HtmlNode table in tables)
                if (table.SelectNodes("thead/tr/th") != null && table.SelectNodes("thead/tr/th").Count >= 3)
                    if (table.SelectNodes("thead/tr/th")[2].InnerText == "4.0 traineddata")
                        foreach (HtmlNode tableBody in table.SelectNodes("tbody"))
                            foreach (HtmlNode row in tableBody.SelectNodes("tr"))
                            {
                                Console.WriteLine("New Row");
                                LanguageEntry currentEntry = new LanguageEntry();
                                currentEntry.shortCode = row.SelectNodes("th|td")[0].InnerText;
                                currentEntry.longName = row.SelectNodes("th|td")[1].InnerText;
                                currentEntry.datapackURL = row.SelectNodes("th|td")[2].Descendants("a").FirstOrDefault().Attributes["href"].Value;
                                languageEntries.Add(currentEntry);
                            }

            //foreach(LanguageEntry entry in languageEntries)
            //{
            //    Console.WriteLine("{0}:{1}:{2}", entry.shortCode, entry.longName, entry.datapackURL);
            //}

            availableLanguagesList = new List<LanguageEntry>(languageEntries);
            listBox_available_langs.ItemsSource = availableLanguagesList;
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("add button was clicked");
        }

        private void btn_add_all_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("add all button was clicked");

            string messageBoxText = "Are you sure you wish add all languages? Language packs are very large in size (10mb+ EACH) and adding all language packs will result in downloading ~1GB of data";
            string caption = "Warning";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
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
