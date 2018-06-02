using Days.Helper;
using Days.Model;
using System;
using System.Collections.ObjectModel;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Days
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class addPage : Page
    {
        public ObservableCollection<CoverEvents> CoverEventsCollection;
        public bool checkInit = false;
        public bool checkDateChanged = true;
        public bool checkComboBoxChanged = true;

        public addPage()
        {
            this.InitializeComponent();
            CoverEventsCollection = CoverEventsManager.GetCoverEvents();
            if (!CoverPageToggle.IsOn)
            {
                swapCoverEventListView.IsEnabled = false;
                newCoverEventRadioButton.IsEnabled = false;
                swapCoverEventRadioButton.IsEnabled = false;
            }
            checkInit = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(coverPage));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (title.Text == "" || date.Date == null || catagory.SelectedItem == null)
            {
                DisplayErrorDialog();
            }
            else if (CoverPageToggle.IsOn && newCoverEventRadioButton.IsChecked == false && swapCoverEventRadioButton.IsChecked == false)
            {
                DisplayNewCEErrorDialog();
            }
            else if (swapCoverEventRadioButton.IsChecked == true && swapCoverEventListView.SelectedItem == null)
            {
                DisplaySwapErrorDialog();
            }
            else
            {
                var selected = catagory.SelectedItem as ComboBoxItem;
                string eventTitle = title.Text;
                DateTimeOffset eventDate = date.Date;

                if (selected == basicEvent)
                {
                    EventsManager.addBasicEvents(eventTitle, eventDate);
                    EventsManager.addBasicEventsDays();
                    EventsManager.WriteBasicEventsData();
                }
                else if (selected == lifeEvent)
                {
                    EventsManager.addLifeEvents(eventTitle, eventDate);
                    EventsManager.addLifeEventsDays();
                    EventsManager.WriteLifeEventsData();
                }
                else if (selected == birthdayEvent)
                {
                    EventsManager.addBirthdayEvents(eventTitle, eventDate);
                    EventsManager.addBirthdayEventsDays();
                    EventsManager.WriteBirthdayEventsData();
                }
                else if (selected == loveEvent)
                {
                    EventsManager.addLoveEvents(eventTitle, eventDate);
                    EventsManager.addLoveEventsDays();
                    EventsManager.WriteLoveEventsData();
                }
                else if (selected == festivalEvent)
                {
                    EventsManager.addFestivalEvents(eventTitle, eventDate);
                    EventsManager.addFestivalEventsDays();
                    EventsManager.WriteFestivalEventsData();
                }
                else if (selected == entertainmentEvent)
                {
                    EventsManager.addEntertainmentEvents(eventTitle, eventDate);
                    EventsManager.addEntertainmentEventsDays();
                    EventsManager.WriteEntertainmentEventsData();
                }
                else if (selected == studyEvent)
                {
                    EventsManager.addStudyEvents(eventTitle, eventDate);
                    EventsManager.addStudyEventsDays();
                    EventsManager.WriteStudyEventsData();
                }
                else if (selected == workEvent)
                {
                    EventsManager.addWorkEvents(eventTitle, eventDate);
                    EventsManager.addWorkEventsDays();
                    EventsManager.WriteWorkEventsData();
                }
                else if (selected == otherEvent)
                {
                    EventsManager.addOtherEvents(eventTitle, eventDate);
                    EventsManager.addOtherEventsDays();
                    EventsManager.WriteOtherEventsData();
                }

                if (CoverPageToggle.IsOn)
                {
                    if (newCoverEventRadioButton.IsChecked == true)
                    {
                        CoverEventsManager.AddCoverEvents(eventTitle, eventDate);
                    }
                    else if (swapCoverEventRadioButton.IsChecked == true && swapCoverEventListView.SelectedItem != null)
                    {
                        int index = swapCoverEventListView.SelectedIndex;
                        CoverEventsManager.AddCoverEvents(eventTitle, eventDate, index);
                    }
                    CoverEventsManager.WriteCoverEventsCollectionData();
                    
                    if (Tile.tileStatus)
                    {
                        Tile.UpdateTile();
                    }
                }

                this.Frame.Navigate(typeof(coverPage));
            }
        }

        private async void DisplayErrorDialog()
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            ContentDialog errorDialog = new ContentDialog()
            {
                Title = resourceLoader.GetString("AddErrorTitle"),
                Content = resourceLoader.GetString("AddErrorContent"),
                CloseButtonText = resourceLoader.GetString("AddErrorClose")
            };

            await errorDialog.ShowAsync();
        }

        private async void DisplaySwapErrorDialog()
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            ContentDialog errorDialog = new ContentDialog()
            {
                Title = resourceLoader.GetString("AddErrorTitle"),
                Content = resourceLoader.GetString("AddErrorContent2"),
                CloseButtonText = resourceLoader.GetString("AddErrorClose")
            };

            await errorDialog.ShowAsync();
        }

        private async void DisplayNewCEErrorDialog()
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            ContentDialog errorDialog = new ContentDialog()
            {
                Title = resourceLoader.GetString("AddErrorTitle"),
                Content = resourceLoader.GetString("AddErrorContent3"),
                CloseButtonText = resourceLoader.GetString("AddErrorClose")
            };

            await errorDialog.ShowAsync();
        }

        private void CoverPageToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (checkInit)
            {
                ToggleSwitch toggleSwitch = sender as ToggleSwitch;
                if (toggleSwitch != null)
                {
                    if (toggleSwitch.IsOn)
                    {
                        int eventsNum = CoverEventsManager.CoverEventsCollection.Count;
                        if (eventsNum >= 5)
                        {
                            newCoverEventRadioButton.IsEnabled = false;
                            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                            newCoverEventBlock.Text = resourceLoader.GetString("CEMaxError");
                        }
                        else
                        {
                            newCoverEventRadioButton.IsEnabled = true;
                        }
                        swapCoverEventRadioButton.IsEnabled = true;
                    }
                    else
                    {
                        newCoverEventRadioButton.IsEnabled = false;
                        swapCoverEventRadioButton.IsEnabled = false;
                        swapCoverEventListView.IsEnabled = false;
                    }
                }
            }
        }

        private void swapCoverEventRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            swapCoverEventListView.IsEnabled = true;
        }

        private void newCoverEventRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            swapCoverEventListView.IsEnabled = false;
        }

        private void daysTextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            if (checkInit)
            {
                int index = args.NewText.Length - 1;
                if (index >= 0)
                {
                    string newText = args.NewText.Substring(index);
                    if (newText == "1" || newText == "2" || newText == "3" || newText == "4" || newText == "5" || newText == "6" || newText == "7" || newText == "8" || newText == "9" || newText == "0")
                    {
                        if (args.NewText.Length > 5)
                        {
                            args.Cancel = true;
                        }
                    }
                    else
                    {
                        args.Cancel = true;
                    }
                }
            }
        }

        private void daysTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {            
            if (checkInit)
            {
                if (checkComboBoxChanged)
                {
                    setNewDate();
                }
                else
                {
                    checkComboBoxChanged = true;
                }
            }
        }

        private void setNewDate()
        {
            checkDateChanged = false;
            if (daysTextBox.Text != "")
            {
                string validatedText = getValidatedText();
                if (validatedText != "0")
                {
                    int validatedDays = Convert.ToInt32(validatedText);
                    if (checkComboBox.SelectedIndex == 0)
                    {
                        validatedDays = -validatedDays;
                    }
                    DateTimeOffset newDate = DateTimeOffset.Now.AddDays(validatedDays);
                    date.Date = newDate;
                }
                else
                {
                    date.Date = DateTimeOffset.Now;                    
                }
            }
            else
            {
                date.Date = DateTimeOffset.Now;
            }
        }

        private string getValidatedText()
        {
            string rawText = daysTextBox.Text;
            int length = rawText.Length;
            string validatedText = "";
            if (length > 1 && rawText.StartsWith('0'))
            {
                if (allCharAreZero(rawText))
                {
                    return "0";
                }
                else
                {
                    char[] rawTextArray = rawText.ToCharArray();
                    int stringIndex = 0;
                    for (int i = 0; i < length; ++i)
                    {
                        if (rawTextArray[i] != '0')
                        {
                            stringIndex = i;
                            break;
                        }
                    }
                    validatedText = rawText.Substring(stringIndex);
                    return validatedText;
                }
            }
            else
            {
                validatedText = rawText;
                return validatedText;
            }
        }

        private bool allCharAreZero(string rawText)
        {
            bool check = true;
            char[] rawTextArray = rawText.ToCharArray();
            for (int i = 0; i <rawText.Length; ++i)
            {
                if (rawTextArray[i] != '0')
                {
                    check = false;
                    return check;
                }
            }
            return check;
        }

        private void checkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkInit)
            {  
                if (checkComboBoxChanged)
                {
                    setNewDate();
                }
            }
        }

        private void date_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            if (checkDateChanged)
            {
                checkComboBoxChanged = false;
                DateTimeOffset selectedDate = date.Date;
                TimeSpan span = selectedDate.Date - DateTimeOffset.Now.Date;
                if (selectedDate.Date > DateTimeOffset.Now.Date)
                {
                    checkComboBox.SelectedIndex = 1;
                    string newText = span.Days.ToString();
                    daysTextBox.Text = newText;
                }
                else if (selectedDate.Date < DateTimeOffset.Now.Date)
                {
                    checkComboBox.SelectedIndex = 0;
                    string newText = span.Negate().Days.ToString();
                    daysTextBox.Text = newText;
                }
                else
                {
                    daysTextBox.Text = "0";
                }
            }
            else
            {
                checkDateChanged = true;
            }
            
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!MemoryCleaner.isLockDown)
            {
                MemoryCleaner.FreeUpMemory();
            }
        }

        private void GoBack_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            this.Frame.Navigate(typeof(coverPage));
            args.Handled = true;
        }

        private void SaveEvent_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            Button_Click_1(sender, null);
            args.Handled = true;
        }
    }
}
