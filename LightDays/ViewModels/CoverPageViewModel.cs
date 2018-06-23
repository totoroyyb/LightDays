using Days.Model;
using System.Collections.ObjectModel;

namespace Days.ViewModels
{
    public class CoverPageViewModel : ViewModelBase
    {
        private ObservableCollection<CoverEvents> _coverEventsCollection { get; set; }
        public ObservableCollection<CoverEvents> CoverEventsCollection
        {
            set
            {
                _coverEventsCollection = value;
                OnPropertyChanged();
            }

            get
            {
                return _coverEventsCollection;
            }
        }
    }
}
