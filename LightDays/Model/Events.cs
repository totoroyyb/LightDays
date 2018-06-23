using System;
using System.Collections.ObjectModel;
using Windows.Storage;
using Days.Constant;

namespace Days.Model
{
    public class Events
    {
        public string title { get; set; }
        public DateTime date { get; set; }
        public TimeSpan days { get; set; }
        public string check { get; set; }
    }

    public class EventsManager
    {
        public static ObservableCollection<Events> basicEvents = new ObservableCollection<Events>();
        public static ObservableCollection<Events> lifeEvents = new ObservableCollection<Events>();
        public static ObservableCollection<Events> loveEvents = new ObservableCollection<Events>();
        public static ObservableCollection<Events> birthdayEvents = new ObservableCollection<Events>();
        public static ObservableCollection<Events> festivalEvents = new ObservableCollection<Events>();
        public static ObservableCollection<Events> entertainmentEvents = new ObservableCollection<Events>();
        public static ObservableCollection<Events> studyEvents = new ObservableCollection<Events>();
        public static ObservableCollection<Events> workEvents = new ObservableCollection<Events>();
        public static ObservableCollection<Events> otherEvents = new ObservableCollection<Events>();

        public static ObservableCollection<Events> GetEventsByFoldIndex(int index)
        {
            switch (index)
            {
                case FoldIndexConstants.basicEvent:
                    addBasicEventsDays();
                    return basicEvents;

                case FoldIndexConstants.lifeEvent:
                    addLifeEventsDays();
                    return lifeEvents;

                case FoldIndexConstants.loveEvent:
                    addLoveEventsDays();
                    return loveEvents;

                case FoldIndexConstants.birthdayEvent:
                    addBirthdayEventsDays();
                    return birthdayEvents;

                case FoldIndexConstants.festivalEvent:
                    addFestivalEventsDays();
                    return festivalEvents;

                case FoldIndexConstants.entertainmentEvent:
                    addEntertainmentEventsDays();
                    return entertainmentEvents;

                case FoldIndexConstants.studyEvent:
                    addStudyEventsDays();
                    return studyEvents;

                case FoldIndexConstants.workEvent:
                    addWorkEventsDays();
                    return workEvents;

                case FoldIndexConstants.otherEvent:
                    addOtherEventsDays();
                    return otherEvents;

                default:
                    return null;
            }
        }

        public static void RemoveOutDatedBasicEvents()
        {
            addBasicEventsDays();
            bool change = false;
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            for (int i = 0; i < basicEvents.Count; ++i)
            {
                if (basicEvents[i].check == resourceLoader.GetString("PastTag"))
                {
                    basicEvents.RemoveAt(i);
                    change = true;
                }
            }

            if (change)
            {
                WriteBasicEventsData();
            }
        }

        public static void RemoveOutDatedLifeEvents()
        {
            addLifeEventsDays();
            bool change = false;
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            for (int i = 0; i < lifeEvents.Count; ++i)
            {
                if (lifeEvents[i].check == resourceLoader.GetString("PastTag"))
                {
                    lifeEvents.RemoveAt(i);
                    change = true;
                }
            }

            if (change)
            {
                WriteLifeEventsData();
            }
        }

        public static void RemoveOutDatedLoveEvents()
        {
            addLoveEventsDays();
            bool change = false;
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            for (int i = 0; i < loveEvents.Count; ++i)
            {
                if (loveEvents[i].check == resourceLoader.GetString("PastTag"))
                {
                    loveEvents.RemoveAt(i);
                    change = true;
                }
            }

            if (change)
            {
                WriteLoveEventsData();
            }
        }

        public static void RemoveOutDatedBirthdayEvents()
        {
            addBirthdayEventsDays();
            bool change = false;
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            for (int i = 0; i < birthdayEvents.Count; ++i)
            {
                if (birthdayEvents[i].check == resourceLoader.GetString("PastTag"))
                {
                    birthdayEvents.RemoveAt(i);
                    change = true;
                }
            }

            if (change)
            {
                WriteBirthdayEventsData();
            }

        }

        public static void RemoveOutDatedFestivalEvents()
        {
            addFestivalEventsDays();
            bool change = false;
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            for (int i = 0; i < festivalEvents.Count; ++i)
            {
                if (festivalEvents[i].check == resourceLoader.GetString("PastTag"))
                {
                    festivalEvents.RemoveAt(i);
                    change = true;
                }
            }

            if (change)
            {
                WriteFestivalEventsData();
            }
        }

        public static void RemoveOutDatedEntertainmentEvents()
        {
            addEntertainmentEventsDays();
            bool change = false;
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            for (int i = 0; i < entertainmentEvents.Count; ++i)
            {
                if (entertainmentEvents[i].check == resourceLoader.GetString("PastTag"))
                {
                    entertainmentEvents.RemoveAt(i);
                    change = true;
                }
            }

            if (change)
            {
                WriteEntertainmentEventsData();
            }
        }

        public static void RemoveOutDatedStudyEvents()
        {
            addStudyEventsDays();
            bool change = false;
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            for (int i = 0; i < studyEvents.Count; ++i)
            {
                if (studyEvents[i].check == resourceLoader.GetString("PastTag"))
                {
                    studyEvents.RemoveAt(i);
                    change = true;
                }
            }

            if (change)
            {
                WriteStudyEventsData();
            }
        }

        public static void RemoveOutDatedWorkEvents()
        {
            addWorkEventsDays();
            bool change = false;
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            for (int i = 0; i < workEvents.Count; ++i)
            {
                if (workEvents[i].check == resourceLoader.GetString("PastTag"))
                {
                    workEvents.RemoveAt(i);
                    change = true;
                }
            }

            if (change)
            {
                WriteWorkEventsData();
            }
        }

        public static void RemoveOutDatedOtherEvents()
        {
            addOtherEventsDays();
            bool change = false;
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            for (int i = 0; i < otherEvents.Count; ++i)
            {
                if (otherEvents[i].check == resourceLoader.GetString("PastTag"))
                {
                    otherEvents.RemoveAt(i);
                    change = true;
                }
            }

            if (change)
            {
                WriteOtherEventsData();
            }
        }

        public static void addBasicEvents(string eventTitle, DateTimeOffset eventDate)
        {
            basicEvents.Add(new Events { title = eventTitle, date = eventDate.Date });
        }

        public static ObservableCollection<Events> getBasicEvents()
        {
            addBasicEventsDays();
            return basicEvents;
        }

        public static void addBasicEventsDays()
        {
            foreach (var eachEvent in basicEvents)
            {
                TimeSpan span = eachEvent.date - DateTimeOffset.Now.Date;
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
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

        public async static void WriteBasicEventsData()
        {
            string dataString = ObjectSerializer.ToXml(basicEvents);
            if (!string.IsNullOrEmpty(dataString))
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile basicEventsFile = await localFolder.CreateFileAsync("basicEventsFile.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(basicEventsFile, dataString);
            }
        }

        public static void addLifeEvents(string eventTitle, DateTimeOffset eventDate)
        {
            lifeEvents.Add(new Events { title = eventTitle, date = eventDate.Date });
        }

        public static ObservableCollection<Events> getLifeEvents()
        {
            addLifeEventsDays();
            return lifeEvents;
        }

        public static void addLifeEventsDays()
        {
            foreach (var eachEvent in lifeEvents)
            {
                TimeSpan span = eachEvent.date - DateTimeOffset.Now.Date;
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
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

        public async static void WriteLifeEventsData()
        {
            string dataString = ObjectSerializer.ToXml(lifeEvents);
            if (!string.IsNullOrEmpty(dataString))
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile lifeEventsFile = await localFolder.CreateFileAsync("lifeEventsFile.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(lifeEventsFile, dataString);
            }
        }

        public static void addBirthdayEvents(string eventTitle, DateTimeOffset eventDate)
        {
            birthdayEvents.Add(new Events { title = eventTitle, date = eventDate.Date });
        }

        public static ObservableCollection<Events> getBirthdayEvents()
        {
            addBirthdayEventsDays();
            return birthdayEvents;
        }

        public static void addBirthdayEventsDays()
        {
            foreach (var eachEvent in birthdayEvents)
            {
                TimeSpan span = eachEvent.date - DateTimeOffset.Now.Date;
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
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

        public async static void WriteBirthdayEventsData()
        {
            string dataString = ObjectSerializer.ToXml(birthdayEvents);
            if (!string.IsNullOrEmpty(dataString))
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile birthdayEventsFile = await localFolder.CreateFileAsync("birthdayEventsFile.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(birthdayEventsFile, dataString);
            }
        }

        public static void addLoveEvents(string eventTitle, DateTimeOffset eventDate)
        {
            loveEvents.Add(new Events { title = eventTitle, date = eventDate.Date });
        }

        public static ObservableCollection<Events> getLoveEvents()
        {
            addLoveEventsDays();
            return loveEvents;
        }

        public static void addLoveEventsDays()
        {
            foreach (var eachEvent in loveEvents)
            {
                TimeSpan span = eachEvent.date - DateTimeOffset.Now.Date;
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
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

        public async static void WriteLoveEventsData()
        {
            string dataString = ObjectSerializer.ToXml(loveEvents);
            if (!string.IsNullOrEmpty(dataString))
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile loveEventsFile = await localFolder.CreateFileAsync("loveEventsFile.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(loveEventsFile, dataString);
            }
        }

        public static void addFestivalEvents(string eventTitle, DateTimeOffset eventDate)
        {
            festivalEvents.Add(new Events { title = eventTitle, date = eventDate.Date });
        }

        public static ObservableCollection<Events> getFestivalEvents()
        {
            addFestivalEventsDays();
            return festivalEvents;
        }

        public static void addFestivalEventsDays()
        {
            foreach (var eachEvent in festivalEvents)
            {
                TimeSpan span = eachEvent.date - DateTimeOffset.Now.Date;
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
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

        public async static void WriteFestivalEventsData()
        {
            string dataString = ObjectSerializer.ToXml(festivalEvents);
            if (!string.IsNullOrEmpty(dataString))
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile festivalEventsFile = await localFolder.CreateFileAsync("festivalEventsFile.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(festivalEventsFile, dataString);
            }
        }

        public static void addEntertainmentEvents(string eventTitle, DateTimeOffset eventDate)
        {
            entertainmentEvents.Add(new Events { title = eventTitle, date = eventDate.Date });
        }

        public static ObservableCollection<Events> getEntertainmentEvents()
        {
            addEntertainmentEventsDays();
            return entertainmentEvents;
        }

        public static void addEntertainmentEventsDays()
        {
            foreach (var eachEvent in entertainmentEvents)
            {
                TimeSpan span = eachEvent.date - DateTimeOffset.Now.Date;
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
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

        public async static void WriteEntertainmentEventsData()
        {
            string dataString = ObjectSerializer.ToXml(entertainmentEvents);
            if (!string.IsNullOrEmpty(dataString))
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile entertainmentEventsFile = await localFolder.CreateFileAsync("entertainmentEventsFile.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(entertainmentEventsFile, dataString);
            }
        }

        public static void addStudyEvents(string eventTitle, DateTimeOffset eventDate)
        {
            studyEvents.Add(new Events { title = eventTitle, date = eventDate.Date });
        }

        public static ObservableCollection<Events> getStudyEvents()
        {
            addStudyEventsDays();
            return studyEvents;
        }

        public static void addStudyEventsDays()
        {
            foreach (var eachEvent in studyEvents)
            {
                TimeSpan span = eachEvent.date - DateTimeOffset.Now.Date;
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
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

        public async static void WriteStudyEventsData()
        {
            string dataString = ObjectSerializer.ToXml(studyEvents);
            if (!string.IsNullOrEmpty(dataString))
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile studyEventsFile = await localFolder.CreateFileAsync("studyEventsFile.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(studyEventsFile, dataString);
            }
        }

        public static void addWorkEvents(string eventTitle, DateTimeOffset eventDate)
        {
            workEvents.Add(new Events { title = eventTitle, date = eventDate.Date });
        }

        public static ObservableCollection<Events> getWorkEvents()
        {
            addWorkEventsDays();
            return workEvents;
        }

        public static void addWorkEventsDays()
        {
            foreach (var eachEvent in workEvents)
            {
                TimeSpan span = eachEvent.date - DateTimeOffset.Now.Date;
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
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

        public async static void WriteWorkEventsData()
        {
            string dataString = ObjectSerializer.ToXml(workEvents);
            if (!string.IsNullOrEmpty(dataString))
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile workEventsFile = await localFolder.CreateFileAsync("workEventsFile.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(workEventsFile, dataString);
            }
        }

        public static void addOtherEvents(string eventTitle, DateTimeOffset eventDate)
        {
            otherEvents.Add(new Events { title = eventTitle, date = eventDate.Date });
        }

        public static ObservableCollection<Events> getOtherEvents()
        {
            addOtherEventsDays();
            return otherEvents;
        }

        public static void addOtherEventsDays()
        {
            foreach (var eachEvent in otherEvents)
            {
                TimeSpan span = eachEvent.date - DateTimeOffset.Now.Date;
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
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

        public async static void WriteOtherEventsData()
        {
            string dataString = ObjectSerializer.ToXml(otherEvents);
            if (!string.IsNullOrEmpty(dataString))
            {
                Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile otherEventsFile = await localFolder.CreateFileAsync("otherEventsFile.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(otherEventsFile, dataString);
            }
        }
    }
}
