﻿using Days.Constant;
using Days.Helper;
using Days.Model;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
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
        private ViewModels.FoldPageModelView viewModel { get; set; }
        private static ScalarKeyFrameAnimation blurIncreaseAnimation;
        private static ScalarKeyFrameAnimation blurDecreaseAnimation;
        private static DropShadow dropShadow;

        public foldPage()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) =>
            {
                viewModel = DataContext as ViewModels.FoldPageModelView;
            };
            Init();
            
        }

        private void Init()
        {
            RegisterHandler();
            InitializeDropShadow(shadowHost, shadowTarget);
        }

        private void RegisterHandler()
        {
            settingPage.OnChangeMottoVisibility += SettingPage_OnChangeMottoVisibility;
        }

        private void SettingPage_OnChangeMottoVisibility(object source, System.EventArgs e)
        {
            viewModel.isMottoShown = UserSettings.isMottoShown;
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
            settingPage.OnChangeMottoVisibility -= SettingPage_OnChangeMottoVisibility;
            if (!MemoryCleaner.isLockDown)
            {
                MemoryCleaner.FreeUpMemory();
            }            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.isMottoShown = UserSettings.isMottoShown;
        }

        private void foldListView_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            dropShadow.StartAnimation("BlurRadius", blurIncreaseAnimation);
        }

        private void foldListView_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            dropShadow.StartAnimation("BlurRadius", blurDecreaseAnimation);
        }
    }
}
