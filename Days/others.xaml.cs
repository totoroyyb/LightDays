using Days.Constant;
using Days.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class others : Page
    {
        public ObservableCollection<Events> eventList;
        public delegate void MyEventHandler(object source, EventArgs e);
        public static event MyEventHandler OnNavigateParentReady;

        public others()
        {
            this.InitializeComponent();
            eventList = EventsManager.getOtherEvents();
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
            EventsManager.otherEvents.RemoveAt(index);
            EventsManager.WriteOtherEventsData();
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

            Events editedEvent = EventsManager.otherEvents[itemIndex];

            EditedEvent.SetEditedEvent(editedEvent);

            SelectedFold.selectedFoldIndex = FoldIndexConstants.otherEvent;

            SelectedEventIndex.selectedItemIndex = itemIndex;
            OnNavigateParentReady(this, null);
        }
    }
}
