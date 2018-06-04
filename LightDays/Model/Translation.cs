using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;

namespace Days.Model
{
    public class Translation
    {
        public static int LangIndex = 1;

        public static void getSelectedIndex()
        {
            IReadOnlyList<string> langList = ApplicationLanguages.Languages;
            string firstLang = langList[0];
            if (firstLang == "zh-Hans-CN" || firstLang == "zh-Hans-SG" || firstLang == "zh-Hans-MO" || firstLang == "zh-Hans-HK")
            {
                LangIndex = 0;
            }
            else
            {
                LangIndex = 1;
            }
        }

        public static void WriteLangData()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["LangData"] = LangIndex;
        }

        public static void ReadLangData()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object langData = localSettings.Values["LangData"];
            if (langData != null)
            {
                LangIndex = (int)langData;
            }
        }
    }
}
