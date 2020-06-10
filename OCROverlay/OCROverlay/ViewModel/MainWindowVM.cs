using OCROverlay.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCROverlay.ViewModel
{
    public class MainWindowVM : BaseViewModel
    {
        public MainWindowVM()
        {
            ScreenSetup();
            TesseractSetup();
        }

        public void ScreenSetup()
        {
            Screen screen = Screen.AllScreens.Where(x => x.DeviceName == Properties.Settings.Default.ChosenScreen).FirstOrDefault();
            Screen = screen;
            Width = screen.Bounds.Width;
            Height = screen.Bounds.Height;
        }

        public void TesseractSetup()
        {
            var solutionDirectory = TryGetSolutionDirectoryInfo()?.FullName;
            if (solutionDirectory == null)
            {
                Console.WriteLine("Could not find the solution folder.");
                return;
            }

            Console.WriteLine(solutionDirectory);

            var tesseractPath = solutionDirectory + @"\Tesseract";
            var testFiles = Directory.EnumerateFiles(solutionDirectory + @"\sampleImages");

            var maxDegreeOfParallelism = Environment.ProcessorCount;
            Parallel.ForEach(testFiles, new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, (fileName) =>
            {
                var imageFile = File.ReadAllBytes(fileName);
                var text = ParseText(tesseractPath, imageFile, "eng", "jpn");
                Console.WriteLine("File:" + fileName + "\n" + text + "\n");
            });
        }

        private static string ParseText(string tesseractPath, byte[] imageFile, params string[] lang)
        {
            string output = string.Empty;
            var tempOutputFile = Path.GetTempPath() + Guid.NewGuid();
            var tempImageFile = Path.GetTempFileName();

            try
            {
                File.WriteAllBytes(tempImageFile, imageFile);

                ProcessStartInfo info = new ProcessStartInfo();
                info.WorkingDirectory = tesseractPath;
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.UseShellExecute = false;
                info.FileName = "cmd.exe";
                info.Arguments =
                    "/c google.tesseract.tesseract-master.exe " + // Image file
                    "--tessdata-dir " + Settings.Default.DownloadLocation + //Datapack location
                    "" + //Image location (/image)
                    "" + //Output image (hocr) file name
                    "-l" + string.Join("+", lang) + //Languages
                    "hocr"; //Text position for everything OCR'

                // Start tesseract.
                Process process = Process.Start(info);
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    // Exit code: success.
                    output = File.ReadAllText(tempOutputFile + ".txt");
                }
                else
                {
                    throw new Exception("Error. Tesseract stopped with an error code = " + process.ExitCode);
                }
            }
            finally
            {
                File.Delete(tempImageFile);
                File.Delete(tempOutputFile + ".txt");
            }

            return output;
        }

        private static DirectoryInfo TryGetSolutionDirectoryInfo()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
                directory = directory.Parent;
            return directory;
        }

        #region Variables

        private Screen _screen;
        public Screen Screen
        {
            get { return _screen; }
            set
            {
                if (value == _screen)
                    return;
                _screen = value;
                NotifyPropertyChanged("Screen");
            }
        }

        private int _width = 0;
        public int Width
        {
            get { return _width; }
            set
            {
                if (value == _width)
                    return;
                _width = value;
                NotifyPropertyChanged("Width");
            }
        }

        private int _height = 0;
        public int Height
        {
            get { return _height; }
            set
            {
                if (value == _height)
                    return;
                _height = value;
                NotifyPropertyChanged("Height");
            }
        }

        #endregion //Variables

        #region Redirects

        #endregion //Redirects

        #region CanExecute

        #endregion //CanExecute

        #region RelayCommands

        #endregion //RelayCommands
    }
}
