using Days.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Days.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        private ObservableCollection<CoverEvents> _coverEventsCollection { get; set; }
        public ObservableCollection<CoverEvents> CoverEventsCollection
        {
            get { return _coverEventsCollection; }
            set
            {
                _coverEventsCollection = value;
                OnPropertyChanged();
            }
        }

        private string _updateInfoText { get; set; }
        public string UpdateInfoText
        {
            get { return _updateInfoText; }
            set
            {
                _updateInfoText = value;
                OnPropertyChanged();
            }
        }
    }
}
