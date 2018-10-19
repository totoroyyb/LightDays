using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;


namespace Days.Model
{
    class Tile
    {
        public static bool tileStatus = false;


        public static void UpdateTile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

            if (AutoDelete.AutoDeleteStatus)
            {
                AutoDelete.DeleteOutDatedCoverEvents();
            }

            CoverEventsManager.AddCoverEventsDays();
            CoverEventsManager.WriteCoverEventsCollectionData();


            ClearTile();

            foreach (var eachEvent in CoverEventsManager.CoverEventsCollection)
            {
                string url = eachEvent.imageUrl;
                if (url != null)
                {
                    createTileUpdate(eachEvent, url);
                }
                else
                {
                    createTileUpdate(eachEvent);
                }
            }
        }

        public static void UpdateTileBG()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object AutoDeleteData = localSettings.Values["AutoDeleteData"];
            Object coverEventsCollection = localSettings.Values["coverEventsCollection"];

            if (coverEventsCollection != null)
            {
                String dataString = (string)coverEventsCollection;
                CoverEventsManager.CoverEventsCollection = ObjectSerializer.CoverEventsFromXml(dataString);
            }

            if (AutoDeleteData != null)
            {
                if ((bool)AutoDeleteData)
                {
                    AutoDelete.DeleteOutDatedCoverEvents();
                }
            }


            CoverEventsManager.AddCoverEventsDaysTileBG();
            CoverEventsManager.WriteCoverEventsCollectionData();


            ClearTile();

            foreach (var eachEvent in CoverEventsManager.CoverEventsCollection)
            {
                string url = eachEvent.imageUrl;
                if (url != null)
                {
                    createTileUpdate(eachEvent, url);
                }
                else
                {
                    createTileUpdate(eachEvent);
                }
            }
        }

        public static void createTileUpdate(CoverEvents coverEvent, string url)
        {
            string eventTitle = coverEvent.title;
            string eventDate = coverEvent.date.ToShortDateString();
            string eventDays = coverEvent.days.Days.ToString();
            string eventCheck = coverEvent.check;
            string imageUrl = url;

            string content = $@"
<tile>
    <visual branding='nameAndLogo'>

        <binding template='TileMedium' displayName='{eventDate}'
                 hint-lockDetailedStatus1='Meeting with Thomas'
                 hint-lockDetailedStatus2='11:00 AM - 12:30 PM'
                 hint-lockDetailedStatus3='Studio F'>
 
            <image src='{imageUrl}' placement='background' hint-overlay='30'/>
            <text hint-style='title' hint-align='center'>{eventDays}</text> 
            <text hint-style='caption' hint-wrap='true' hint-align='center' hint-maxLines='2'>{eventTitle}</text>
        </binding>

        <binding template='TileWide' displayName='{eventDate}'
                 hint-lockDetailedStatus1='Meeting with Thomas'
                 hint-lockDetailedStatus2='11:00 AM - 12:30 PM'
                 hint-lockDetailedStatus3='Studio F'>

            <image src='{imageUrl}' placement='background' hint-overlay='30'/>
            <group>
                <subgroup hint-weight='5'>
                    <text hint-style='subheader' hint-align='center'>{eventDays}</text>
                    <text hint-style='body' hint-align='center'>{eventCheck}</text>
                </subgroup>

                <subgroup hint-weight='6' hint-textStacking='center'>
                    <text hint-style='body' hint-wrap='true' hint-align='center'>{eventTitle}</text>
                </subgroup>
            </group>
        </binding>

        <binding template='TileLarge'>
            <image src='{imageUrl}' placement='background' hint-overlay='30'/>
            <text hint-style='header' hint-align='center'>{eventDays}</text>
            <text hint-style='subtitle' hint-align='center'>{eventDate}</text>
            <text hint-style='body' hint-align='center'>{eventCheck}</text>
            <text hint-style='subtitle' hint-wrap='true' hint-align='center' hint-maxLines='2'>{eventTitle}</text>
        </binding>

    </visual>
</tile>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);

            TileNotification tileNotification = new TileNotification(doc);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public static void createTileUpdate(CoverEvents coverEvent)
        {
            string eventTitle = coverEvent.title;
            string eventDate = coverEvent.date.ToShortDateString();
            string eventDays = coverEvent.days.Days.ToString();
            string eventCheck = coverEvent.check;

            string content = $@"
<tile>
    <visual branding='nameAndLogo'>

        <binding template='TileMedium' displayName='{eventDate}'>
            <text hint-style='title' hint-align='center'>{eventDays}</text> 
            <text hint-style='caption' hint-wrap='true' hint-align='center' hint-maxLines='2'>{eventTitle}</text>
        </binding>

        <binding template='TileWide' displayName='{eventDate}'>
            <group>
                <subgroup hint-weight='5'>
                    <text hint-style='subheader' hint-align='center'>{eventDays}</text>
                    <text hint-style='bodySubtle' hint-align='center'>{eventCheck}</text>
                </subgroup>

                <subgroup hint-weight='6' hint-textStacking='center'>
                    <text hint-style='body' hint-wrap='true' hint-align='center'>{eventTitle}</text>
                </subgroup>
            </group>
        </binding>

        <binding template='TileLarge'>
            <text hint-style='header' hint-align='center'>{eventDays}</text>
            <text hint-style='subtitle' hint-align='center'>{eventDate}</text>
            <text hint-style='bodySubtle' hint-align='center'>{eventCheck}</text>
            <text hint-style='subtitle' hint-wrap='true' hint-align='center' hint-maxLines='2'>{eventTitle}</text>
        </binding>

    </visual>
</tile>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);

            TileNotification tileNotification = new TileNotification(doc);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public static void ClearTile()
        {
            Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Clear();
        }

    }
}
