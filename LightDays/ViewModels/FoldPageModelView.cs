using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Days.ViewModels
{
    public class FoldPageModelView : ViewModelBase
    {
        private bool _isMottoShown { get; set; }
        public bool isMottoShown
        {
            get { return _isMottoShown; }
            set
            {
                _isMottoShown = value;
                OnPropertyChanged();
            }
        }
    }
}
