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
        private static readonly string ThemeSettingName = "ThemeSetting";
        private static readonly string RoundedCornerSettingName = "RoundedCornerSetting";
        private static readonly string MottoSwitchSettingName = "MottoSwitchSetting";

        public static ElementTheme Theme = ElementTheme.Default;
        public static bool isRounded = true;
        public static bool isMottoShown = true;

        private static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        #region ThemeMethods
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
        #endregion

        #region RoundedCornerMethod
        public static void SetRoundedCorner(bool isOn)
        {
            SaveRoundedSettings(isOn);
            isRounded = isOn;
        }

        public static void SaveRoundedSettings(bool isOn)
        {
            WriteSettings(RoundedCornerSettingName, isOn);
        }

        public static void LoadRoundedSettings()
        {
            isRounded = ReadSettingsBool(RoundedCornerSettingName);
        }
        #endregion

        #region
        public static void SetMotto(bool isOn)
        {
            SaveMottoSettings(isOn);
            isMottoShown = isOn;
        }

        public static void SaveMottoSettings(bool isOn)
        {
            WriteSettings(MottoSwitchSettingName, isOn);
        }

        public static void LoadMottoSettings()
        {
            isMottoShown = ReadSettingsBool(MottoSwitchSettingName);
        }
        #endregion


        #region WriteAndReadSettingsMethod
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

        private static void WriteSettings(string settingName, bool settingBool)
        {
            localSettings.Values[settingName] = settingBool;
        }

        private static bool ReadSettingsBool(string settingName)
        {
            Object value = localSettings.Values[settingName];
            if (value != null)
            {
                return (bool)value;
            }
            return true;
        }
        #endregion


    }
}
