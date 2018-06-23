using Days.Model;
using System.Collections.ObjectModel;

namespace Days.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        private ObservableCollection<Events> _eventList { get; set; }
        public ObservableCollection<Events> EventList
        {
            get { return _eventList; }
            set
            {
                _eventList = value;
                OnPropertyChanged();
            }
        }
    }
}
