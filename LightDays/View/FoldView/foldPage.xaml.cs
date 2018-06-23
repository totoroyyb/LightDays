using Days.Constant;
using Days.Helper;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Shapes;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Days
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class foldPage : Page
    {
        public foldPage()
        {
            this.InitializeComponent();
            InitializeDropShadow(shadowHost, shadowTarget);
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

        private void foldListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (basicEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(EventsPage), FoldIndexConstants.basicEvent);
            } else if (lifeEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(EventsPage), FoldIndexConstants.lifeEvent);
            } else if (loveEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(EventsPage), FoldIndexConstants.loveEvent);
            } else if (birthdayEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(EventsPage), FoldIndexConstants.birthdayEvent);
            } else if (festivalEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(EventsPage), FoldIndexConstants.festivalEvent);
            } else if (entertainmentEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(EventsPage), FoldIndexConstants.entertainmentEvent);
            } else if (studyEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(EventsPage), FoldIndexConstants.studyEvent);
            } else if (workEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(EventsPage), FoldIndexConstants.workEvent);
            } else if (otherEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(EventsPage), FoldIndexConstants.otherEvent);
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!MemoryCleaner.isLockDown)
            {
                MemoryCleaner.FreeUpMemory();
            }            
        }
    }
}
