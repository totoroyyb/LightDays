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

        private bool _isRounded { get; set; }
        public bool IsRounded
        {
            set
            {
                _isRounded = value;
                OnPropertyChanged();
            }

            get { return _isRounded; }
        }
    }
}
