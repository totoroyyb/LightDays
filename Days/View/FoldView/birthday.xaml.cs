using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Days.Model;
using System.Collections.ObjectModel;
using Days.Constant;
using Days.Helper;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Days
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class birthday : Page
    {
        public ObservableCollection<Events> eventList;
        public delegate void MyEventHandler(object source, EventArgs e);
        public static event MyEventHandler OnNavigateParentReady;

        public birthday()
        {
            this.InitializeComponent();
            eventList = EventsManager.getBirthdayEvents();
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
            EventsManager.birthdayEvents.RemoveAt(index);
            EventsManager.WriteBirthdayEventsData();
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

            Events editedEvent = EventsManager.birthdayEvents[itemIndex];

            EditedEvent.SetEditedEvent(editedEvent);

            SelectedFold.selectedFoldIndex = FoldIndexConstants.birthdayEvent;

            SelectedEventIndex.selectedItemIndex = itemIndex;
            OnNavigateParentReady(this, null);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!MemoryCleaner.isLockDown)
            {
                eventList = null;
            }
        }
    }
}
