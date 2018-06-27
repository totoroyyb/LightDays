using Days.Helper;
using Days.Model;
using Microsoft.Services.Store.Engagement;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Days
{
    sealed partial class App : Application
    {
        public App()
        {
            ReadThemeData();
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            RegisterNotificationChannelAsync();
            TryToRegisterBGT();
            LoadUserSettings();
        }

        #region Init Register
        public async void TryToRegisterBGT()
        {
            try
            {
                await Task.Run(() =>
                {
                    ResetRegisterRequest();
                    RegisterBackgroundTask();
                });
            }
            catch (Exception)
            {

            }
        }

        private async void RegisterNotificationChannelAsync()
        {
            StoreServicesEngagementManager manager = StoreServicesEngagementManager.GetDefault();
            await manager.RegisterNotificationChannelAsync();
        }

        public async void ResetRegisterRequest()
        {
            BackgroundExecutionManager.RemoveAccess();
            await BackgroundExecutionManager.RequestAccessAsync();
        }

        public void RegisterBackgroundTask()
        {
            string tileUpdateTaskName = "tileUpdateTaskName";

            bool register = false;

            foreach (var backgroundTask in BackgroundTaskRegistration.AllTasks)
            {
                if (backgroundTask.Value.Name == tileUpdateTaskName)
                {
                    register = true;
                    break;
                }
            }

            if (!register)
            {
                var builder = new BackgroundTaskBuilder();
                builder.Name = "tileUpdateTaskName";
                builder.SetTrigger(new TimeTrigger(360, false));

                BackgroundTaskRegistration task = builder.Register();
            }
        }
        #endregion

        #region Background Task
        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);
            IBackgroundTaskInstance taskInstance = args.TaskInstance;

            try
            {
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                Object coverTileToggleData = localSettings.Values["coverTileToggleData"];

                if (coverTileToggleData != null)
                {
                    bool coverTileToggleStatus = (bool)coverTileToggleData;
                    if (coverTileToggleStatus)
                    {
                        Tile.UpdateTileBG();
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            ReadLangData();
            ReadCoverInfo();
            ReadAllEventsInfo();
            Password.ReadWinHelloData();

            Frame rootFrame = Window.Current.Content as Frame;

            
            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    if (Password.winHelloStatus)
                    {
                        rootFrame.Navigate(typeof(checkPassword), e.Arguments);
                    }
                    else
                    {
                        rootFrame.Navigate(typeof(MainPage), e.Arguments);
                    }
                }

                Window.Current.Activate();
                
                extendIntoTitleBar();
            }
        }

        private void LoadUserSettings()
        {
            UserSettings.LoadRoundedSettings();
            UserSettings.LoadMottoSettings();
        }

        private void ReadThemeData()
        {
            int index = UserSettings.GetElementTheme();
            if (index != -1 && index != (int)ElementTheme.Default)
            {
                if (index == (int)ElementTheme.Light)
                {
                    RequestedTheme = ApplicationTheme.Light;
                }
                else if (index == (int)ElementTheme.Dark)
                {
                    RequestedTheme = ApplicationTheme.Dark;
                }
            }
        }

        private void ReadLangData()
        {
            Translation.getSelectedIndex();
            Translation.WriteLangData();
        }

        private void extendIntoTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            if (CheckContract.isAPIContractExist(6))
            {
                var viewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
                viewTitleBar.ButtonBackgroundColor = Colors.Transparent;
                viewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                viewTitleBar.ButtonForegroundColor = (Color)Resources["SystemBaseHighColor"];
            }
            else
            {
                ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }
        }

        private void ReadCoverInfo()
        {
            ReadAutoDeleteData();
            ReadCoverEventsData();
            ReadCoverSource();
        }

        private async void ReadAllEventsInfo()
        {
            await Task.Run(() =>
            {
                ReadBasicEventsData();
                ReadLifeEventsData();
                ReadBirthdayEventsData();
                ReadLoveEventsData();
                ReadFestivalEventsData();
                ReadEntertainmentEventsData();
                ReadStudyEventsData();
                ReadWorkEventsData();
                ReadOtherEventsData();

                if (AutoDelete.AutoDeleteStatus)
                {
                    AutoDelete.DeleteOutDatedAllEvents();
                    AutoDelete.DeleteOutDatedCoverEvents();
                }
            });
        }

        #region ReadCoverEvents

        private void ReadAutoDeleteData()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object AutoDeleteData = localSettings.Values["AutoDeleteData"];
            if (AutoDeleteData != null)
            {
                AutoDelete.AutoDeleteStatus = (bool)AutoDeleteData;
            }
        }

        private void ReadCoverSource()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object coverSource = localSettings.Values["coverSource"];
            Object CustomizedCBGStatus = localSettings.Values["CustomizedCBGStatus"];

            if (CustomizedCBGStatus != null)
            {
                CustomizedCBG.CustomizedCBGStatus = (bool)CustomizedCBGStatus;
            }

            if (coverSource != null)
            {
                CBGManager.Source.CoverSource = (string)coverSource;
            }

            if (CustomizedCBG.CustomizedCBGStatus)
            {
                CustomizedCBG.ReadCustomizedCBG();
            }
        }

        private void ReadCoverEventsData()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object coverEventTitle = localSettings.Values["coverEventTitle"];
            Object coverEventDate = localSettings.Values["coverEventDate"];

            Object coverEventsCollection = localSettings.Values["coverEventsCollection"];

            Object coverTileToggleData = localSettings.Values["coverTileToggleData"];

            if (coverEventTitle != null && coverEventDate != null)
            {
                string eventTitle = (string)coverEventTitle;
                DateTime eventDate = DateTime.Parse((string)coverEventDate);
                CoverEventsManager.AddCoverEvents(eventTitle, eventDate);
                localSettings.Values.Remove("coverEventTitle");
                localSettings.Values.Remove("coverEventDate");
            }
            else
            {
                if (coverEventsCollection != null)
                {
                    String dataString = (string)coverEventsCollection;
                    CoverEventsManager.CoverEventsCollection = ObjectSerializer.CoverEventsFromXml(dataString);
                }
                else
                {
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                    string eventTitle = resourceLoader.GetString("DefaultCoverTitle");
                    DateTime eventDate = DateTimeOffset.Now.Date;
                    CoverEventsManager.AddCoverEvents(eventTitle, eventDate);
                    CoverEventsManager.WriteCoverEventsCollectionData();
                }
            }

            if (coverTileToggleData != null)
            {
                Tile.tileStatus = (bool)coverTileToggleData;
                if (Tile.tileStatus)
                {
                    Tile.UpdateTile();
                }
            }

        }

        #endregion

        #region ReadEventsData

        private async void ReadBasicEventsData()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile basicEventsFile = await localFolder.GetFileAsync("basicEventsFile.txt");
                String dataString = await FileIO.ReadTextAsync(basicEventsFile);
                EventsManager.basicEvents = ObjectSerializer.FromXml(dataString);
            }
            catch (Exception)
            {

            }

        }

        private async void ReadLifeEventsData()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile lifeEventsFile = await localFolder.GetFileAsync("lifeEventsFile.txt");
                String dataString = await FileIO.ReadTextAsync(lifeEventsFile);
                EventsManager.lifeEvents = ObjectSerializer.FromXml(dataString);
            }
            catch (Exception)
            {

            }

        }

        private async void ReadBirthdayEventsData()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile birthdayEventsFile = await localFolder.GetFileAsync("birthdayEventsFile.txt");
                String dataString = await FileIO.ReadTextAsync(birthdayEventsFile);
                EventsManager.birthdayEvents = ObjectSerializer.FromXml(dataString);
            }
            catch (Exception)
            {

            }

        }

        private async void ReadLoveEventsData()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile loveEventsFile = await localFolder.GetFileAsync("loveEventsFile.txt");
                String dataString = await FileIO.ReadTextAsync(loveEventsFile);
                EventsManager.loveEvents = ObjectSerializer.FromXml(dataString);
            }
            catch (Exception)
            {

            }

        }

        private async void ReadFestivalEventsData()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile festivalEventsFile = await localFolder.GetFileAsync("festivalEventsFile.txt");
                String dataString = await FileIO.ReadTextAsync(festivalEventsFile);
                EventsManager.festivalEvents = ObjectSerializer.FromXml(dataString);
            }
            catch (Exception)
            {

            }

        }

        private async void ReadEntertainmentEventsData()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile entertainmentEventsFile = await localFolder.GetFileAsync("entertainmentEventsFile.txt");
                String dataString = await FileIO.ReadTextAsync(entertainmentEventsFile);
                EventsManager.entertainmentEvents = ObjectSerializer.FromXml(dataString);
            }
            catch (Exception)
            {

            }

        }

        private async void ReadStudyEventsData()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile studyEventsFile = await localFolder.GetFileAsync("studyEventsFile.txt");
                String dataString = await FileIO.ReadTextAsync(studyEventsFile);
                EventsManager.studyEvents = ObjectSerializer.FromXml(dataString);
            }
            catch (Exception)
            {

            }

        }

        private async void ReadWorkEventsData()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile workEventsFile = await localFolder.GetFileAsync("workEventsFile.txt");
                String dataString = await FileIO.ReadTextAsync(workEventsFile);
                EventsManager.workEvents = ObjectSerializer.FromXml(dataString);
            }
            catch (Exception)
            {

            }
        }

        private async void ReadOtherEventsData()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile otherEventsFile = await localFolder.GetFileAsync("otherEventsFile.txt");
                String dataString = await FileIO.ReadTextAsync(otherEventsFile);
                EventsManager.otherEvents = ObjectSerializer.FromXml(dataString);
            }
            catch (Exception)
            {

            }
        }

        #endregion

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
