using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCROverlay.Model
{
    public class DetectedWord
    {
        public string Word { get; set; }
        public string Class { get; set; }
        public string Id { get; set; }
        public string FullTitle { get; set; }
        public int TopStartingPosition { get; set; }
        public int BottomEndingPosition { get; set; }
        public int LeftStartingPosition { get; set; }
        public int RightEndingPosition { get; set; }
    }
}
