using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Days.Model
{
    public class AutoDelete
    {
        public static bool AutoDeleteStatus = false;

        public static void DeleteOutDatedCoverEvents()
        {
            ObservableCollection<CoverEvents> coverEvents = CoverEventsManager.GetCoverEvents();
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            for (int i = 0; i < coverEvents.Count; ++i)
            {
                if (coverEvents[i].check == resourceLoader.GetString("PastTag"))
                {
                    CoverEventsManager.CoverEventsCollection.RemoveAt(i);
                    CoverEventsManager.ResetCoverEventsHeader(i);
                }
            }

            if (CoverEventsManager.CoverEventsCollection.Count == 0)
            {
                string eventTitle = resourceLoader.GetString("DefaultCoverTitle");
                DateTime eventDate = DateTimeOffset.Now.Date;
                CoverEventsManager.AddCoverEvents(eventTitle, eventDate);
                CoverEventsManager.AddCoverEventsDays();
            }

            CoverEventsManager.WriteCoverEventsCollectionData();
        }

        public static void DeleteOutDatedAllEvents()
        {
            EventsManager.RemoveOutDatedBasicEvents();
            EventsManager.RemoveOutDatedBirthdayEvents();
            EventsManager.RemoveOutDatedEntertainmentEvents();
            EventsManager.RemoveOutDatedFestivalEvents();
            EventsManager.RemoveOutDatedLifeEvents();
            EventsManager.RemoveOutDatedLoveEvents();
            EventsManager.RemoveOutDatedOtherEvents();
            EventsManager.RemoveOutDatedStudyEvents();
            EventsManager.RemoveOutDatedWorkEvents();
        }

        public static void WriteAutoDeleteData()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["AutoDeleteData"] = AutoDeleteStatus;
        }
    }
}
