using Days.Model;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Days.Helper
{
    public class UpdateInfoGrabber
    {
        private static readonly string EnUpdateFileName = @"Update_en.txt";
        private static readonly string ZhUpdateFileName = @"Update_zh.txt";

        public async static Task<string> GrabUpdateInfoByLang()
        {
            int langIndex = Translation.LangIndex;
            if (langIndex == 0)
            {
                return await ReadUpdateInfoFromFile(ZhUpdateFileName);
            }
            else
            {
                return await ReadUpdateInfoFromFile(EnUpdateFileName);
            }
        }

        private async static Task<string> ReadUpdateInfoFromFile(string fileName)
        {
            //Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            //Windows.Storage.StorageFile file = await storageFolder.GetFileAsync(fileName);
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///UpdateInfo/" + fileName));
            string text = await FileIO.ReadTextAsync(file);
            return text;
        }
    }
}
