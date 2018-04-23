using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Days.Model
{
    public class CoverEvents
    {
        public string title { get; set; }
        public DateTime date { get; set; }
        public TimeSpan days { get; set; }
        public string check { get; set; }
        public string header { get; set; }
        public string imageUrl { get; set; }
    }

    public class CoverEventsManager
    {
        public static ObservableCollection<CoverEvents> CoverEventsCollection = new ObservableCollection<CoverEvents>();

        public static void AddCoverEvents(string eventTitle, DateTimeOffset eventDate, int Index)
        {
            string eventHeader = CalHeader(Index);
            CoverEventsCollection[Index] = new CoverEvents { title = eventTitle, date = eventDate.Date, header = eventHeader };
        }

        public static void AddCoverEvents(string eventTitle, DateTimeOffset eventDate)
        {
            int eventsNum = CoverEventsCollection.Count;
            string eventHeader = CalHeader(eventsNum);
            CoverEventsCollection.Add(new CoverEvents { title = eventTitle, date = eventDate.Date, header = eventHeader });
        }

        public static ObservableCollection<CoverEvents> GetCoverEvents()
        {
            AddCoverEventsDays();
            return CoverEventsCollection;
        }

        public static void AddCoverEventsDays()
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            foreach (var eachEvent in CoverEventsCollection)
            {
                TimeSpan span = eachEvent.date - DateTimeOffset.Now.Date;

                if (eachEvent.date > DateTimeOffset.Now.Date)
                {
                    eachEvent.days = span;
                    eachEvent.check = resourceLoader.GetString("FutureTag");
                }
                else if (eachEvent.date == DateTimeOffset.Now.Date)
                {
                    eachEvent.days = span;
                    eachEvent.check = resourceLoader.GetString("NowTag");
                }
                else if (eachEvent.date < DateTimeOffset.Now.Date)
                {
                    eachEvent.days = span.Negate();
                    eachEvent.check = resourceLoader.GetString("PastTag");
                }
            }
        }

        public static void AddCoverEventsDaysTileBG()
        {
            Translation.ReadLangData();
            int langIndex = Translation.LangIndex;

            foreach (var eachEvent in CoverEventsCollection)
            {
                TimeSpan span = eachEvent.date - DateTimeOffset.Now.Date;
                
                if (eachEvent.date > DateTimeOffset.Now.Date)
                {
                    eachEvent.days = span;

                    if (langIndex == 0)
                    {
                        eachEvent.check = "[未发生]";
                    }
                    else
                    {
                        eachEvent.check = "[Future]";
                    }
                }
                else if (eachEvent.date == DateTimeOffset.Now.Date)
                {
                    eachEvent.days = span;
                    if (langIndex == 0)
                    {
                        eachEvent.check = "[今天]";
                    }
                    else
                    {
                        eachEvent.check = "[Today]";
                    }
                }
                else if (eachEvent.date < DateTimeOffset.Now.Date)
                {
                    eachEvent.days = span.Negate();
                    if (langIndex == 0)
                    {
                        eachEvent.check = "[已过去]";
                    }
                    else
                    {
                        eachEvent.check = "[Past]";
                    }
                }
            }
        }

        public static void ResetCoverEventsHeader(int index)
        {
            int eventsNum = CoverEventsCollection.Count;
            for (int count = index; count < eventsNum; ++count)
            {
                CoverEvents oddCoverEvent = CoverEventsCollection[count];
                CoverEvents newCoverEvent = oddCoverEvent;
                newCoverEvent.header = CalHeader(count);
                CoverEventsCollection[count] = newCoverEvent;
            }
        }

        public static string CalHeader(int Index)
        {
            string header = "";
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            switch (Index)
            {
                case 0:
                    header = resourceLoader.GetString("Cover1");
                    break;

                case 1:
                    header = resourceLoader.GetString("Cover2");
                    break;

                case 2:
                    header = resourceLoader.GetString("Cover3");
                    break;

                case 3:
                    header = resourceLoader.GetString("Cover4");
                    break;

                case 4:
                    header = resourceLoader.GetString("Cover5");
                    break;
            }
            return header;
        }

        public static void WriteCoverEventsCollectionData()
        {
            string dataString = ObjectSerializer.CoverEventsToXml(CoverEventsCollection);
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["coverEventsCollection"] = dataString;
        }

        public static void SetImageUrl(string url, int index)
        {
            if (url == null)
            {
                CoverEventsCollection[index].imageUrl = null;
            }
            else
            {
                string finalUrl = "Assets/TileBG/" + url + ".jpg";
                CoverEventsCollection[index].imageUrl = finalUrl;
            }
        }
    }


}
