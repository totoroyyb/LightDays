using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Days.Model
{
    public class UserSettings
    {
        private const string ThemeSettingName = "ThemeSetting";
        public static ElementTheme Theme = ElementTheme.Default;

        private static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public static void SetElementTheme(ElementTheme theme)
        {
            SaveThemeData((int)theme);
            Theme = theme;
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.RequestedTheme = theme;
        }

        public static int GetElementTheme()
        {
            int index = ReadSettingsInt(ThemeSettingName);
            switch (index)
            {
                case (int)ElementTheme.Light:
                    Theme = ElementTheme.Light;
                    break;

                case (int)ElementTheme.Dark:
                    Theme = ElementTheme.Dark;
                    break;
            }
            return index;
        }

        public static void SaveThemeData(int index)
        {
            WriteSettings(ThemeSettingName, index);
        }

        private static void WriteSettings(string settingName, string settingString)
        {
            localSettings.Values[settingName] = settingString;
        }

        private static string ReadSettingsString(string settingName)
        {
            Object value = localSettings.Values[settingName];
            if (value != null)
            {
                return (string)value;
            }
            return null;
        }

        private static void WriteSettings(string settingName, int settingInt)
        {
            localSettings.Values[settingName] = settingInt;
        }

        private static int ReadSettingsInt(string settingName)
        {
            Object value = localSettings.Values[settingName];
            if (value != null)
            {
                return (int)value;
            }
            return -1;
        }


    }
}
