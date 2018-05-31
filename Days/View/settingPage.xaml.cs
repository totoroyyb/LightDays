using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Days.Model;
using Windows.UI.Popups;
using Windows.Security.Credentials;
using Windows.System;
using System.Collections.ObjectModel;
using Windows.Globalization;
using Days.Helper;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Days
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class settingPage : Page
    {
        public bool checkInit = false;
        public delegate void MyEventHandler(object source, EventArgs e);
        public static event MyEventHandler OnNavigateParentReady;
        public static event MyEventHandler SetCBGImageReady;
        public ObservableCollection<CoverEvents> CoverEventsCollection;

        public settingPage()
        {
            this.InitializeComponent();
            coverTileToggle.IsOn = Tile.tileStatus;
            WHToggle.IsOn = Password.winHelloStatus;
            autoDelete.IsOn = AutoDelete.AutoDeleteStatus;
            selectLang.SelectedIndex = Translation.LangIndex;
            SetLockButtonState();
            CoverEventsCollection = CoverEventsManager.GetCoverEvents();
            EditTileBGButton.IsEnabled = false;
            checkInit = true;
        }

        private void SetLockButtonState()
        {
            if (Password.winHelloStatus)
            {
                LockButton.IsEnabled = true;
            }
            else
            {
                LockButton.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(coverPage));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selected = CBGGridView.SelectedItem as GridViewItem;
            if (selected != null)
            {
                string fileName = selected.Name;
                CBGManager.SetCoverSource(fileName);
                SetCBGImageReady(this, null);
            }
            else
            {
                DisplayErrorDialog();
            }

        }

        private async void DisplayErrorDialog()
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            ContentDialog errorDialog = new ContentDialog()
            {
                Title = resourceLoader.GetString("CBGErrorTitle"),
                Content = resourceLoader.GetString("CBGErrorContent"),
                CloseButtonText = resourceLoader.GetString("CBGErrorClose")
            };

            await errorDialog.ShowAsync();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (checkInit)
            {
                ToggleSwitch toggleSwitch = sender as ToggleSwitch;
                if (toggleSwitch != null)
                {
                    if (toggleSwitch.IsOn == true)
                    {
                        Tile.tileStatus = true;
                        Tile.UpdateTile();
                        saveCoverTileToggleData(true);
                    }
                    else
                    {
                        Tile.tileStatus = false;
                        Tile.ClearTile();
                        saveCoverTileToggleData(false);
                    }
                }
            }

        }

        private void saveCoverTileToggleData(bool status)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            localSettings.Values["coverTileToggleData"] = status;
        }

        private void WHToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (checkInit)
            {
                ToggleSwitch toggleSwitch = sender as ToggleSwitch;
                if (toggleSwitch != null)
                {
                    if (toggleSwitch.IsOn == true && !Password.winHelloStatus)
                    {
                        tryToOpenWinHello();
                    }
                    else if (toggleSwitch.IsOn == false && Password.winHelloStatus)
                    {
                        tryToCloseWinHello();
                        Password.winHelloStatus = false;
                    }
                }
            }
        }

        private async void tryToOpenWinHello()
        {
            KeyCredentialRetrievalResult keyCreationResult = await KeyCredentialManager.RequestCreateAsync("WinHello", KeyCredentialCreationOption.ReplaceExisting);
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            if (keyCreationResult.Status == KeyCredentialStatus.Success)
            {
                Password.winHelloStatus = true;
                LockButton.IsEnabled = true;
                Password.WriteWinHelloData();
            }
            else if (keyCreationResult.Status == KeyCredentialStatus.NotFound)
            {
                Password.winHelloStatus = false;
                WHToggle.IsOn = false;
                MessageDialog message = new MessageDialog(resourceLoader.GetString("WHError1"));
                await message.ShowAsync();
            }
            else if (keyCreationResult.Status == KeyCredentialStatus.UnknownError)
            {
                Password.winHelloStatus = false;
                WHToggle.IsOn = false;
                MessageDialog message = new MessageDialog(resourceLoader.GetString("WHError2"));
                await message.ShowAsync();
            }
            else
            {
                Password.winHelloStatus = false;
                WHToggle.IsOn = false;
            }
        }

        private async void tryToCloseWinHello()
        {
            KeyCredentialRetrievalResult keyCreationResult = await KeyCredentialManager.RequestCreateAsync("WinHello", KeyCredentialCreationOption.ReplaceExisting);
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            if (keyCreationResult.Status == KeyCredentialStatus.Success)
            {
                Password.winHelloStatus = false;
                LockButton.IsEnabled = false;
                Password.WriteWinHelloData();
            }
            else if (keyCreationResult.Status == KeyCredentialStatus.NotFound)
            {
                Password.winHelloStatus = true;
                WHToggle.IsOn = true;
                MessageDialog message = new MessageDialog(resourceLoader.GetString("WHError1"));
                await message.ShowAsync();
            }
            else if (keyCreationResult.Status == KeyCredentialStatus.UnknownError)
            {
                Password.winHelloStatus = true;
                WHToggle.IsOn = true;
                MessageDialog message = new MessageDialog(resourceLoader.GetString("WHError2"));
                await message.ShowAsync();
            }
            else
            {
                Password.winHelloStatus = true;
                WHToggle.IsOn = true;
            }
        }

        //lock down app
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OnNavigateParentReady(this, null);
        }

        private void settingPageFrame_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape)
            {
                this.Frame.Navigate(typeof(coverPage));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            if (CoverEventsManager.CoverEventsCollection.Count == 1)
            {
                DeleteInfo.Text = resourceLoader.GetString("CEManagerInfo1");
                ConfirmButton.IsEnabled = false;
            }
            else if (CoverEventsManagerListView.SelectedItem != null)
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
            int index = CoverEventsManagerListView.SelectedIndex;
            CoverEventsManager.CoverEventsCollection.RemoveAt(index);
            CoverEventsManager.ResetCoverEventsHeader(index);
            CoverEventsManager.WriteCoverEventsCollectionData();

            if (Tile.tileStatus)
            {
                Tile.UpdateTile();
            }
        }

        private void EditTileBGButton_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            if (!Tile.tileStatus)
            {
                CoverTilesGridView.IsEnabled = false;
                SaveButton.IsEnabled = false;
                EditTileBGInfo.Text = resourceLoader.GetString("TileBGInfo1");
            }
            else
            {
                CoverTilesGridView.IsEnabled = true;
                SaveButton.IsEnabled = true;
                EditTileBGInfo.Text = resourceLoader.GetString("TileBGInfo2");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            EditTileBGFlyout.Hide();
            var selected = CoverTilesGridView.SelectedItem as GridViewItem;
            if (selected != null)
            {
                int index = CoverEventsManagerListView.SelectedIndex;
                string url = selected.Name;
                if (url == "Default")
                {
                    CoverEventsManager.SetImageUrl(null, index);
                }
                else
                {
                    CoverEventsManager.SetImageUrl(url, index);
                }

                Tile.UpdateTile();
                CoverEventsManager.WriteCoverEventsCollectionData();
            }

        }

        private void CoverEventsManagerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CoverEventsManagerListView.SelectedItem != null)
            {
                EditTileBGButton.IsEnabled = true;
            }
        }

        private void autoDelete_Toggled(object sender, RoutedEventArgs e)
        {
            if (checkInit)
            {
                ToggleSwitch toggleSwitch = sender as ToggleSwitch;
                if (toggleSwitch != null)
                {
                    if (toggleSwitch.IsOn == true)
                    {
                        AutoDelete.AutoDeleteStatus = true;
                        AutoDelete.DeleteOutDatedAllEvents();
                        AutoDelete.WriteAutoDeleteData();
                        if (Tile.tileStatus)
                        {
                            Tile.UpdateTile();
                        }
                    }
                    else
                    {
                        AutoDelete.AutoDeleteStatus = false;
                        AutoDelete.WriteAutoDeleteData();
                    }
                }
            }
        }

        private void CustomizeCBG_Click(object sender, RoutedEventArgs e)
        {
            CustomizedCBG.SetCustomizedCBG();
        }

        private void selectLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkInit)
            {
                ComboBox langSelector = sender as ComboBox;
                if (langSelector.SelectedIndex == 0)
                {
                    ApplicationLanguages.PrimaryLanguageOverride = "zh-CN";
                    Translation.LangIndex = 0;
                }
                else
                {
                    ApplicationLanguages.PrimaryLanguageOverride = "en-US";
                    Translation.LangIndex = 1;
                }
                Translation.WriteLangData();
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();

                CoverEventsManager.ResetCoverEventsHeader(0);
                CoverEventsManager.WriteCoverEventsCollectionData();
                DisplayLangDialog();
            }
        }

        private async void DisplayLangDialog()
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            ContentDialog errorDialog = new ContentDialog()
            {
                Title = resourceLoader.GetString("LangInfoTitle"),
                Content = resourceLoader.GetString("LangInfoContent"),
                CloseButtonText = resourceLoader.GetString("LangInfoClose")
            };

            await errorDialog.ShowAsync();
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
