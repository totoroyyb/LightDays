using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;

namespace Days.Model
{
    public class SelectedEvent
    {
        public string title { get; set; }
        public DateTime date { get; set; }
        public TimeSpan days { get; set; }
        public string check { get; set; }
    }

    public class SelectedEventManager
    {
        public static SelectedEvent coverEvent = new SelectedEvent { title = ResourceLoader.GetForCurrentView().GetString("DefaultCoverTitle"), date = DateTimeOffset.Now.Date};

        public static void setCoverEvent(string eventTitle, DateTimeOffset eventDate)
        {
            coverEvent.title = eventTitle;
            coverEvent.date = eventDate.Date;
            addCoverEventDays();
            WriteCoverEventData();
        }

        public static SelectedEvent getCoverEvent()
        {
            addCoverEventDays();
            return coverEvent;
        }

        public static void addCoverEventDays()
        {

            TimeSpan span = coverEvent.date - DateTimeOffset.Now.Date;
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            if (coverEvent.date > DateTimeOffset.Now.Date)
            {
                coverEvent.days = span;
                coverEvent.check = resourceLoader.GetString("FutureTag");
            }
            else if (coverEvent.date == DateTimeOffset.Now.Date)
            {
                coverEvent.days = span;
                coverEvent.check = resourceLoader.GetString("NowTag");
            }
            else if (coverEvent.date < DateTimeOffset.Now.Date)
            {
                coverEvent.days = span.Negate();
                coverEvent.check = resourceLoader.GetString("PastTag");
            }
        }

        public static void WriteCoverEventData()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["coverEventTitle"] = coverEvent.title;
            localSettings.Values["coverEventDate"] = coverEvent.date.ToShortDateString();
        } 



    }
}
