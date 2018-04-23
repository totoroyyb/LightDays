using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
            if (keyCreationResult.Status == KeyCredentialStatus.Success)
            {
                checkPasswordFrame.Navigate(typeof(MainPage));
            }
            else if (keyCreationResult.Status == KeyCredentialStatus.NotFound)
            {
                MessageDialog message = new MessageDialog("暂时无法使用Windows Hello，请先在系统内配置Windows Hello");
                await message.ShowAsync();
            }
            else if (keyCreationResult.Status == KeyCredentialStatus.UnknownError)
            {
                MessageDialog message = new MessageDialog("您的设备不支持Windows Hello，或其配置有误，出现未知错误");
                await message.ShowAsync();
            }
        }

        private void checkPasswordFrame_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (IsCtrlKeyPressed())
            {
                if (e.Key == VirtualKey.O)
                {
                    Button_Click(this, null);
                }
            }
        }

        private static bool IsCtrlKeyPressed()
        {
            var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);
            return (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
        }
    }
}
