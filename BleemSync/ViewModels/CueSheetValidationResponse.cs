using System.Collections.Generic;

namespace BleemSync.ViewModels
{
    public class CueSheetValidationResponse
    {
        public bool Valid { get; set; }
        public string Message { get; set; }
        public List<string> BinFiles { get; set; }

        public CueSheetValidationResponse()
        {
            BinFiles = new List<string>();
        }
    }
}
