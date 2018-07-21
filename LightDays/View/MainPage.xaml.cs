using Days.Helper;
using Days.Model;
using System;
using System.Numerics;
using Windows.System;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
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
        private static ScalarKeyFrameAnimation blurIncreaseAnimation;
        private static ScalarKeyFrameAnimation blurDecreaseAnimation;
        private static DropShadow dropShadow;

        public MainPage()
        {
            this.InitializeComponent();
            InitializeDropShadow(shadowHost, shadowTarget);
            TryToSetCBG();            
            coverFrame.Navigate(typeof(coverPage));
            foldFrame.Navigate(typeof(foldPage));
        }

        private void RegisterNaviToEditHandlers()
        {
            EventsPage.OnNavigateParentReady += myControl_OnNavigateParentReady;
        }

        private void myControl_OnNavigateParentReady(object source, EventArgs e)
        {
            coverFrame.Navigate(typeof(EditPage));
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
            dropShadow = compositor.CreateDropShadow();
            //dropShadow.Color = Color.FromArgb(255, 75, 75, 80);
            dropShadow.Color = Colors.Black;
            dropShadow.BlurRadius = 10;
            dropShadow.Offset = new Vector3(0, 0, 0);
            // Associate the shape of the shadow with the shape of the target element
            dropShadow.Mask = shadowTarget.GetAlphaMask();

            CreateAnimation(compositor);

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

        private void CreateAnimation(Compositor compositor)
        {
            blurIncreaseAnimation = Animation.CreateBlurIncreaseAnimation(compositor);
            blurDecreaseAnimation = Animation.CreateBlurDecreaseAnimation(compositor);
        }

        private void ToLockScreen(object source, EventArgs e)
        {
            this.Frame.Navigate(typeof(checkPassword));
        }

        private void SetCoverImage(object source, EventArgs e)
        {
            string newImageUri = "ms-appx:///" + CBGManager.Source.CoverSource;
            BitmapImage bimage = new BitmapImage(new Uri(newImageUri));            
            CBGImage.Source = bimage;
            CustomizedCBG.WriteStatus(false);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterEventHandlers();
            AddKeyAccelerator();
        }

        private void AddKeyAccelerator()
        {
            KeyboardAccelerator lockDown = new KeyboardAccelerator();
            lockDown.Key = VirtualKey.L;
            lockDown.Modifiers = VirtualKeyModifiers.Control;
            lockDown.Invoked += LockDown_Invoked;
            this.KeyboardAccelerators.Add(lockDown);
        }

        private void LockDown_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (Password.winHelloStatus)
            {
                this.Frame.Navigate(typeof(checkPassword));
            }
            args.Handled = true;
        }

        private void RegisterEventHandlers()
        {
            RegisterNaviToEditHandlers();
            settingPage.OnNavigateParentReady += ToLockScreen;
            settingPage.SetCBGImageReady += SetCoverImage;
            CustomizedCBG.OnSetCustomizedCBGReady += SetCustomizedCBG;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            MemoryCleaner.isLockDown = true;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            DeregisterEventHandlers();
            ClearKeyboardAccelerator();
            MemoryCleaner.FreeUpMemory();
        }

        private void ClearKeyboardAccelerator()
        {
            this.KeyboardAccelerators.Clear();
        }

        private void DeregisterEventHandlers()
        {
            DeregisterNaviToEditHandlers();
            settingPage.OnNavigateParentReady -= ToLockScreen;
            settingPage.SetCBGImageReady -= SetCoverImage;
            CustomizedCBG.OnSetCustomizedCBGReady -= SetCustomizedCBG;
        }

        private void DeregisterNaviToEditHandlers()
        {
            EventsPage.OnNavigateParentReady -= myControl_OnNavigateParentReady;
        }

        private void coverFrame_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var CBGVisual = ElementCompositionPreview.GetElementVisual(CBGImageBorder);
            CBGVisual.Scale = new Vector3(1.06f, 1.06f, 0);
        }

        private void coverFrame_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var CBGVisual = ElementCompositionPreview.GetElementVisual(CBGImageBorder);
            CBGVisual.Scale = new Vector3(1f, 1f, 0);
        }

        private void foldFrame_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            dropShadow.StartAnimation("BlurRadius", blurIncreaseAnimation);
        }

        private void foldFrame_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            dropShadow.StartAnimation("BlurRadius", blurDecreaseAnimation);
        }
    }
}
