using Days.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Days
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public CBG Source;

        public MainPage()
        {
            this.InitializeComponent();
            InitializeDropShadow(shadowHost, shadowTarget);
            TryToSetCBG();            
            coverFrame.Navigate(typeof(coverPage));
            foldFrame.Navigate(typeof(foldPage));
            settingPage.OnNavigateParentReady += ToLockScreen;
            settingPage.SetCBGImageReady += SetCoverImage;
            CustomizedCBG.OnSetCustomizedCBGReady += SetCustomizedCBG;
        }

        private void TryToSetCBG()
        {
            if (CustomizedCBG.CustomizedCBGStatus)
            {
                CBGImage.Source = CustomizedCBG.bitmapCBG;
            }
            else
            {
                string newImageUri = "ms-appx:///" + CBGManager.Source.CoverSource;
                BitmapImage bimage = new BitmapImage(new Uri(newImageUri));
                CBGImage.Source = bimage;
            }
        }

        private void SetCustomizedCBG(object image, EventArgs e)
        {
            CBGImage.Source = (BitmapImage)image;
        }

        private void InitializeDropShadow(UIElement shadowHost, Shape shadowTarget)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(shadowHost);
            Compositor compositor = hostVisual.Compositor;

            // Create a drop shadow
            var dropShadow = compositor.CreateDropShadow();
            dropShadow.Color = Color.FromArgb(255, 75, 75, 80);
            dropShadow.BlurRadius = 25;
            dropShadow.Offset = new Vector3(1.0f, 1.0f, 0.0f);
            // Associate the shape of the shadow with the shape of the target element
            dropShadow.Mask = shadowTarget.GetAlphaMask();

            // Create a Visual to hold the shadow
            var shadowVisual = compositor.CreateSpriteVisual();
            shadowVisual.Shadow = dropShadow;

            // Add the shadow as a child of the host in the visual tree
            ElementCompositionPreview.SetElementChildVisual(shadowHost, shadowVisual);

            // Make sure size of shadow host and shadow visual always stay in sync
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);

            shadowVisual.StartAnimation("Size", bindSizeAnimation);
        }


        private void ToLockScreen(object source, EventArgs e)
        {
            MainPageFrame.Navigate(typeof(checkPassword));
        }

        private void MainPageFrame_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (IsCtrlKeyPressed())
            {
                if (e.Key == VirtualKey.L && Password.winHelloStatus)
                {
                    MainPageFrame.Navigate(typeof(checkPassword));
                }
                else if (e.Key == VirtualKey.N)
                {
                    coverFrame.Navigate(typeof(addPage));
                }
                else if (e.Key == VirtualKey.U)
                {
                    coverFrame.Navigate(typeof(settingPage));
                }
            }
        }

        private static bool IsCtrlKeyPressed()
        {
            var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);
            return (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
        }

        private void SetCoverImage(object source, EventArgs e)
        {
            string newImageUri = "ms-appx:///" + CBGManager.Source.CoverSource;
            BitmapImage bimage = new BitmapImage(new Uri(newImageUri));            
            CBGImage.Source = bimage;
            CustomizedCBG.WriteStatus(false);
        }
    }
}
