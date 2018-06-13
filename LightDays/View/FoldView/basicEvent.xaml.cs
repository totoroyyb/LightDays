using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Days.Model;
using System.Collections.ObjectModel;
using Days.Constant;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Days.Helper;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Days
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class basicEvent : Page
    {
        public ObservableCollection<Events> eventList;
        public delegate void MyEventHandler(object source, EventArgs e);
        public static event MyEventHandler OnNavigateParentReady;

        public basicEvent()
        {
            this.InitializeComponent();
            eventList = EventsManager.getBasicEvents();
            eventList.CollectionChanged += EventList_CollectionChanged;
            CheckVisibility.CheckButtonGridVisibility(eventList, ButtonGrid);
        }

        private void EventList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CheckVisibility.CheckButtonGridVisibility(eventList, ButtonGrid);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            if (EventListView.SelectedItem != null)
            {
                DeleteInfo.Text = resourceLoader.GetString("CEManagerInfo2");
                ConfirmButton.IsEnabled = true;
            }
            else
            {
                DeleteInfo.Text = resourceLoader.GetString("CEManagerInfo3");
                ConfirmButton.IsEnabled = false;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteFlyout.Hide();
            int index = EventListView.SelectedIndex;
            EventsManager.basicEvents.RemoveAt(index);
            EventsManager.WriteBasicEventsData();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            if (EventListView.SelectedItem != null)
            {
                EditInfo.Text = resourceLoader.GetString("EditInfo1");
                EditEvent.IsEnabled = true;
            }
            else
            {
                EditInfo.Text = resourceLoader.GetString("EditInfo2");
                EditEvent.IsEnabled = false;
            }
        }

        private void EditEvent_Click(object sender, RoutedEventArgs e)
        {
            EditFlyout.Hide();
            int itemIndex = EventListView.SelectedIndex;

            Events editedEvent = EventsManager.basicEvents[itemIndex];

            EditedEvent.SetEditedEvent(editedEvent);

            SelectedFold.selectedFoldIndex = FoldIndexConstants.basicEvent;

            SelectedEventIndex.selectedItemIndex = itemIndex;

            OnNavigateParentReady(this, null);
        }

        private void EventListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var framework = (FrameworkElement)e.OriginalSource;
            Object dataContext = framework.DataContext;

            if (dataContext != null)
            {
                NewView.dataContext = dataContext;
                RightTabMenuFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
            }
        }

        private async void NewViewButton_Click(object sender, RoutedEventArgs e)
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(SecondaryView), null);
                Window.Current.Content = frame;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });

            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId, ViewSizePreference.UseHalf);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            eventList.CollectionChanged -= EventList_CollectionChanged;

            if (!MemoryCleaner.isLockDown)
            {
                eventList = null;
            }
        }
    }
}
