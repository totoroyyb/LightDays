using Days.Model;
using Microsoft.Services.Store.Engagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Core;

namespace Days
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            RegisterNotificationChannelAsync();
            TryToRegisterBGT();
        }


        public void TryToRegisterBGT()
        {
            try
            {
                ResetRegisterRequest();
                RegisterBackgroundTask();
            }
            catch (Exception)
            {

            }
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

        private async void RegisterNotificationChannelAsync()
        {
            StoreServicesEngagementManager manager = StoreServicesEngagementManager.GetDefault();
            await manager.RegisterNotificationChannelAsync();
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            ReadLangData();
            ReadCoverInfo();
            ReadAllEventsInfo();
            Password.ReadWinHelloData();

            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                //rootFrame.Navigated += OnNavigated;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;

                // Register a handler for BackRequested events and set the
                // visibility of the Back button
                //SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

                //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                //    rootFrame.CanGoBack ?
                //    AppViewBackButtonVisibility.Visible :
                //    AppViewBackButtonVisibility.Collapsed;

            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    if (Password.winHelloStatus)
                    {
                        rootFrame.Navigate(typeof(checkPassword), e.Arguments);
                    }
                    else
                    {
                        rootFrame.Navigate(typeof(MainPage), e.Arguments);
                    }
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();

                extendIntoTitleBar();
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
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        private void ReadCoverInfo()
        {
            ReadAutoDeleteData();
            ReadCoverEventsData();
            ReadCoverSource();
        }

        private void ReadAutoDeleteData()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object AutoDeleteData = localSettings.Values["AutoDeleteData"];
            if (AutoDeleteData != null)
            {
                AutoDelete.AutoDeleteStatus = (bool)AutoDeleteData;
            }
        }

        private void ReadAllEventsInfo()
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



        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        //private void OnNavigated(object sender, NavigationEventArgs e)
        //{
        //    // Each time a navigation event occurs, update the Back button's visibility
        //    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
        //        ((Frame)sender).CanGoBack ?
        //        AppViewBackButtonVisibility.Visible :
        //        AppViewBackButtonVisibility.Collapsed;
        //}

        //private void OnBackRequested(object sender, BackRequestedEventArgs e)
        //{
        //    Frame rootFrame = Window.Current.Content as Frame;

        //    if (rootFrame.CanGoBack)
        //    {
        //        e.Handled = true;
        //        rootFrame.GoBack();
        //    }
        //}


        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
