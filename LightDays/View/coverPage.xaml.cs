using Days.Helper;
using Days.Model;
using System;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.System;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Days
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class coverPage : Page
    {

        public ViewModels.CoverPageViewModel ViewModel { get; set; }

        public coverPage()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) =>
            {
                ViewModel = DataContext as ViewModels.CoverPageViewModel;
            };
            CoverEventsManager.ResetCoverEventsHeader(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(settingPage));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(addPage));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ResetCoverEventFlyout.Hide();

            CoverEventsManager.CoverEventsCollection.Clear();

            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            string eventTitle = resourceLoader.GetString("DefaultCoverTitle");
            DateTime eventDate = DateTimeOffset.Now.Date;
            CoverEventsManager.AddCoverEvents(eventTitle, eventDate);
            CoverEventsManager.WriteCoverEventsCollectionData();

            ViewModel.CoverEventsCollection = CoverEventsManager.GetCoverEvents();

            if (Tile.tileStatus)
            {
                Tile.UpdateTile();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            FeedbackFlyout.Hide();
            var openReview = Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:REVIEW?PFN={0}", Windows.ApplicationModel.Package.Current.Id.FamilyName)));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            FeedbackFlyout.Hide();
            var openReview = Launcher.LaunchUriAsync(new Uri("mailto:totoroq@outlook.com"));
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!MemoryCleaner.isLockDown)
            {
                MemoryCleaner.FreeUpMemory();
            }
        }

        private void OpenSettings_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            this.Frame.Navigate(typeof(settingPage));
            args.Handled = true;
        }

        private void AddNew_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            this.Frame.Navigate(typeof(addPage));
            args.Handled = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.CoverEventsCollection = CoverEventsManager.GetCoverEvents();
            ViewModel.IsRounded = UserSettings.isRounded;
        }
    }
}
