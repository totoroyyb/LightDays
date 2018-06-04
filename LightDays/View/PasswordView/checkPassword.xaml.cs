using Days.Helper;
using System;
using Windows.Security.Credentials;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Days
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class checkPassword : Page
    {
        public checkPassword()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            KeyCredentialRetrievalResult keyCreationResult = await KeyCredentialManager.RequestCreateAsync("WinHello", KeyCredentialCreationOption.ReplaceExisting);
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            if (keyCreationResult.Status == KeyCredentialStatus.Success)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
            else if (keyCreationResult.Status == KeyCredentialStatus.NotFound)
            {
                MessageDialog message = new MessageDialog(resourceLoader.GetString("WHError1"));
                await message.ShowAsync();
            }
            else if (keyCreationResult.Status == KeyCredentialStatus.UnknownError)
            {
                MessageDialog message = new MessageDialog(resourceLoader.GetString("WHError2"));
                await message.ShowAsync();
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            MemoryCleaner.FreeUpMemory();
            MemoryCleaner.isLockDown = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            KeyboardAccelerator unlock = new KeyboardAccelerator();
            unlock.Key = VirtualKey.O;
            unlock.Modifiers = VirtualKeyModifiers.Control;
            unlock.Invoked += Unlock_Invoked;
            this.KeyboardAccelerators.Add(unlock);
        }

        private void Unlock_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            Button_Click(this, null);
            args.Handled = true;
        }
    }
}
