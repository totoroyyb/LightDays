using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Days.Model
{
    public class CBG
    {
        public string CoverSource { get; set; }
    }

    public class CBGManager
    {
        public static CBG Source = new CBG { CoverSource = "Assets/CoverBG/CBG1.jpg"};

        public static void SetCoverSource(string coverStr)
        {
            string path = "Assets/CoverBG/" + coverStr + ".jpg";
            Source.CoverSource = path;
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["coverSource"] = path;
        }

        public static CBG GetCoverSouce()
        {
            return Source;
        }
    }

}
