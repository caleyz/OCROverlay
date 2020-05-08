using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCROverlay.Model
{
    [Serializable()]
    public class LanguageEntry
    {        
        public string LongName { get; set; }
        public string ShortCode { get; set; }
        public string DatapackURL { get; set; }
    }
}
