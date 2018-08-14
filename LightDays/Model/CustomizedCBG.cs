using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Days.Model
{
    public class CustomizedCBG
    {
        public delegate void MyEventHandler(object source, EventArgs e);
        public static event MyEventHandler OnSetCustomizedCBGReady;

        public static bool CustomizedCBGStatus = false;
        public static BitmapImage bitmapCBG = new BitmapImage();
        
        public static async void SetCustomizedCBG()
        {
            var picker = new FileOpenPicker();
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;

            var inputFile = await fileOpenPicker.PickSingleFileAsync();

            if (inputFile == null)
            {
                return;
            }
            else
            {
                var bitmapImage = new BitmapImage();
                using (var stream = await inputFile.OpenAsync(FileAccessMode.Read))
                {
                    await bitmapImage.SetSourceAsync(stream);
                }
                OnSetCustomizedCBGReady(bitmapImage, null);
                bitmapCBG = bitmapImage;

                using (var inputStream = await inputFile.OpenSequentialReadAsync())
                {
                    var readStream = inputStream.AsStreamForRead();
                    byte[] buffer = new byte[readStream.Length];
                    await readStream.ReadAsync(buffer, 0, buffer.Length);

                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFile CustomizedCBG = await localFolder.CreateFileAsync("CustomizedCBG.txt", CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteBytesAsync(CustomizedCBG, buffer);
                }

                WriteStatus(true);
            }
        }

        public static void WriteStatus(bool status)
        {
            CustomizedCBGStatus = status;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["CustomizedCBGStatus"] = status;
        }

        public static async Task ReadCustomizedCBG()
        {
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await localFolder.GetFileAsync("CustomizedCBG.txt");
                byte[] result;
                using (Stream stream = await sampleFile.OpenStreamForReadAsync())
                {
                    using (var memoryStream = new MemoryStream())
                    {

                        stream.CopyTo(memoryStream);
                        result = memoryStream.ToArray();
                    }
                }

                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
                    {
                        writer.WriteBytes(result);
                        await writer.StoreAsync();
                    }
                    BitmapImage image = new BitmapImage();
                    image.SetSource(stream);
                    bitmapCBG = image;
                }
            }
            catch (Exception)
            {

            }

        }
    }
}
