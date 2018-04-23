using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Days.Model
{
    class Password
    {
        public static bool passwordStatus = false;
        public static bool winHelloStatus = false;
        public static string password = null;

        public static void WritePasswordData()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["passwordStatus"] = passwordStatus;
            localSettings.Values["winHelloStatus"] = winHelloStatus;
            localSettings.Values["password"] = password;
        }

        public static void WriteWinHelloData()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["winHelloStatus"] = winHelloStatus;
        }

        public static void ReadWinHelloData()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object winHelloData = localSettings.Values["winHelloStatus"];
            if (winHelloData != null)
            {
                winHelloStatus = (bool)winHelloData;
            }
        }
    }
}
