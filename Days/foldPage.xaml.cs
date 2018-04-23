using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
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
                eventFrame.Navigate(typeof(basicEvent));
            } else if (lifeEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(life));
            } else if (loveEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(love));
            } else if (birthdayEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(birthday));
            } else if (festivalEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(festival));
            } else if (entertainmentEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(entertainment));
            } else if (studyEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(study));
            } else if (workEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(work));
            } else if (otherEvents.IsSelected)
            {
                eventFrame.Navigate(typeof(others));
            }
        }
    }
}
